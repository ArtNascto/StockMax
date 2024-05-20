using Microsoft.AspNetCore.Mvc;
using Minio;
using Minio.DataModel;
using Minio.DataModel.Args;
using Newtonsoft.Json;
using StockMax.API.Utils;
using StockMax.Domain.Interfaces.Services;
using StockMax.Domain.Models.Entity;
using StockMax.Domain.Models.View;
using StockMax.Domain.Models.View.Helpers;

namespace StockMax.API.Controllers
{
    [Route("product")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _service;
        private readonly IConfiguration _config;

        public ProductController(IProductService service, IConfiguration config)
        {
            _service = service;
            _config = config;
        }

        [HttpPost]
        [Route("update")]
        public async Task<IActionResult> Update(ProductDto productDto)
        {
            try
            {
                Guid.TryParse(productDto.Id, out Guid productGuid);
                var product = await _service.Get(productGuid);
                var productPath = Guid.NewGuid();
                if (product == null)
                {
                    return NotFound("Product not found");
                }
                else
                {
                    product.Category = productDto.Category;
                    product.Code = productDto.Code;
                    product.Colors = productDto.Colors;
                    product.Description = productDto.Description;
                    product.Label = productDto.Label;
                    product.LastUpdate = DateTime.Now;
                    product.Name = productDto.Name;
                    product.Quantity = productDto.Quantity;
                    product.Status = productDto.Status;
                    product.Vendor = productDto.Vendor;
                    product = await _service.Update(product);
                }

                return Ok(productDto);
            }
            catch (Exception ex)
            {
                throw new Exception("Error when update product", ex);
            }
        }

        [HttpGet]
        [Route("getall")]
        public async Task<IActionResult> GetAll([FromQuery] QueryParameters queryParameters)
        {
            if (!string.IsNullOrEmpty(queryParameters.OrderBy) && queryParameters.OrderBy.ToLower().Contains("undefined"))
            {
                return StatusCode(400);
            }

            var products = await _service.GetAll(queryParameters);
            var productDto = new List<ProductDto>();
            foreach (var product in products)
            {
                productDto.Add(new ProductDto()
                {
                    Id = product.Id.ToString(),
                    Category = product.Category,
                    Code = product.Code,
                    Colors = product.Colors,
                    CreationTime = product.CreationTime,
                    Description = product.Description,
                    ImagePath = product.ImagePath,
                    Label = product.Label,
                    DeletionTime = product.DeletionTime,
                    LastUpdate = product.LastUpdate,
                    Name = product.Name,
                    Quantity = product.Quantity,
                    Status = product.Status,
                    Vendor = product.Vendor
                });
            }
            var allProductsCount = await _service.Count();
            var paginationMetadata = new
            {
                totalCount = allProductsCount,
                pageSize = queryParameters.PageCount,
                currentPage = queryParameters.Page,
                totalPages = queryParameters.GetTotalPages(allProductsCount)
            };
            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(paginationMetadata));
            var toReturn = productDto.Select(QueryUtil<ProductDto>.ExpandSingleItem);

            return Ok(new
            {
                value = toReturn,
                totalCount = allProductsCount,
                pageSize = queryParameters.PageCount,
                currentPage = queryParameters.Page,
                totalPages = queryParameters.GetTotalPages(allProductsCount)
            });
        }

        [HttpDelete]
        [Route("delete/{id}")]
        public async Task<IActionResult> Delete([FromRoute] string id)
        {
            Guid.TryParse(id, out Guid productGuid);
            var product = await _service.Get(productGuid);
            if (product != null)
            {
                await _service.Delete(productGuid);
            }
            else
            {
                return NotFound("Product not found");
            }
            return Ok();
        }

        [HttpGet]
        [Route("get/{id}")]
        public async Task<IActionResult> Get(string id)
        {
            Guid.TryParse(id, out Guid productGuid);
            var product = await _service.Get(productGuid);

            if (product != null)
            {
                return Ok(new ProductDto()
                {
                    Id = product.Id.ToString(),
                    Category = product.Category,
                    Code = product.Code,
                    Colors = product.Colors,
                    CreationTime = product.CreationTime,
                    Description = product.Description,
                    ImagePath = product.ImagePath,
                    Label = product.Label,
                    DeletionTime = product.DeletionTime,
                    LastUpdate = product.LastUpdate,
                    Name = product.Name,
                    Quantity = product.Quantity,
                    Status = product.Status,
                    Vendor = product.Vendor,
                    Value = product.Value
                });
            }
            return NotFound(new UserDto());
        }

        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> Create(ProductDto productDto)
        {
            try
            {
                Guid.TryParse(productDto.Id, out Guid productGuid);
                var product = await _service.Get(productGuid);
                var productPath = Guid.NewGuid();
                if (product == null)
                {
                    var newProduct = new Product()
                    {
                        Id = productGuid,
                        Category = productDto.Category,
                        Code = productDto.Code,
                        Colors = productDto.Colors,
                        CreationTime = DateTime.Now,
                        Description = productDto.Description ?? string.Empty,
                        ImagePath = string.Empty,
                        Label = productDto.Label ?? string.Empty,
                        LastUpdate = DateTime.Now,
                        Name = productDto.Name,
                        Quantity = productDto.Quantity,
                        Status = productDto.Status,
                        Vendor = productDto.Vendor ?? string.Empty,
                        Value = productDto.Value
                    };
                    product = await _service.Create(newProduct);
                }
                else
                {
                    product.Category = productDto.Category;
                    product.Code = productDto.Code;
                    product.Colors = productDto.Colors;
                    product.Description = productDto.Description ?? string.Empty;
                    product.Label = productDto.Label ?? string.Empty;
                    product.LastUpdate = DateTime.Now;
                    product.Name = productDto.Name;
                    product.Quantity = productDto.Quantity;
                    product.Status = productDto.Status;
                    product.Vendor = productDto.Vendor ?? string.Empty;
                    product.Value = productDto.Value;

                    product = await _service.Update(product);
                }

                return Ok(productDto);
            }
            catch (Exception ex)
            {
                throw new Exception("Error when upload product image", ex);
            }
        }

        [HttpPost]
        [Route("uploadImage/{productId}")]
        [RequestFormLimits(MultipartBodyLengthLimit = int.MaxValue)]
        [RequestSizeLimit(int.MaxValue)]
        public async Task<IActionResult> UploadProductImage(IFormFile image, [FromRoute] string productId)
        {
            try
            {
                if (image == null || image.Length == 0)
                    return BadRequest("Invalid File");
                var storageURL = _config["Storage:URL"];
                var storageUsername = _config["Storage:Username"];
                var storagePassword = _config["Storage:Password"];
                var storageBucket = _config["Storage:Bucket"];
                var minioClient = new MinioClient().WithEndpoint(storageURL).WithCredentials(storageUsername, storagePassword).Build();
                var args = new BucketExistsArgs().WithBucket(storageBucket);
                if (!(await minioClient.BucketExistsAsync(args)))
                {
                    var mbArgs = new MakeBucketArgs().WithBucket(storageBucket);
                    await minioClient.MakeBucketAsync(mbArgs);
                }
                Guid.TryParse(productId, out Guid productGuid);
                var product = await _service.Get(productGuid);
                var productPath = Guid.NewGuid();
                if (product == null)
                {
                    var newProduct = new Product()
                    {
                        Id = productGuid,
                        Category = string.Empty,
                        Code = string.Empty,
                        Colors = string.Empty,
                        CreationTime = DateTime.Now,
                        Description = string.Empty,
                        ImagePath = @$"product/{productPath}",
                        Label = string.Empty,
                        LastUpdate = DateTime.Now,
                        Name = string.Empty,
                        Quantity = 0,
                        Status = string.Empty,
                        Vendor = string.Empty,
                        Value = 0
                    };
                    product = await _service.Create(newProduct);
                }
                else
                {
                    product.ImagePath = @$"product/{productPath}";
                    product = await _service.Update(product);
                }
                var progress = new Progress<ProgressReport>(progressReport =>
                {
                    Console.WriteLine(
                            $"Percentage: {progressReport.Percentage}% TotalBytesTransferred: {progressReport.TotalBytesTransferred} bytes");
                    if (progressReport.Percentage != 100)
                        Console.SetCursorPosition(0, Console.CursorTop - 1);
                    else Console.WriteLine();
                });

                using (var stream = image.OpenReadStream())
                {
                    PutObjectArgs putObjectArgs = new PutObjectArgs()
                                      .WithBucket(storageBucket)
                                      .WithObject(@$"product/{productPath}")
                                      .WithStreamData(stream)
                                      .WithContentType(image.ContentType)
                                      .WithObjectSize(image.Length)
                                      .WithProgress(progress);

                    await minioClient.PutObjectAsync(putObjectArgs);
                }
                return Ok(new ProductDto()
                {
                    Id = product.Id.ToString(),
                    Category = product.Category,
                    Code = product.Code,
                    Colors = product.Colors,
                    CreationTime = product.CreationTime,
                    Description = product.Description,
                    ImagePath = product.ImagePath,
                    Label = product.Label,
                    DeletionTime = product.DeletionTime,
                    LastUpdate = product.LastUpdate,
                    Name = product.Name,
                    Quantity = product.Quantity,
                    Status = product.Status,
                    Vendor = product.Vendor
                });
            }
            catch (Exception ex)
            {
                throw new Exception("Error when upload product image", ex);
            }
        }

        [HttpGet]
        [Route("getColors")]
        public async Task<IActionResult> GetColors()
        {
            var colors = await _service.GetColors();
            return Ok(colors);
        }

        [HttpGet]
        [Route("getProductImage/{productId}")]
        public async Task<IActionResult> GetProductImage([FromRoute] string productId)
        {
            var storageURL = _config["Storage:URL"];
            var storageUsername = _config["Storage:Username"];
            var storagePassword = _config["Storage:Password"];
            var storageBucket = _config["Storage:Bucket"];
            Guid.TryParse(productId, out Guid productGuid);
            var product = await _service.Get(productGuid);

            if (product != null)
            {
                var minioClient = new MinioClient().WithEndpoint(storageURL).WithCredentials(storageUsername, storagePassword).Build();
                try
                {
                    var stat = await GetFileInfo(product.ImagePath);
                    var res = new ReleaseableFileStreamModel
                    {
                        ContentType = stat.Type,
                        FileName = stat.Name,
                    };

                    using (var memoryStream = new MemoryStream())
                    {
                        GetObjectArgs getObjectArgs = new GetObjectArgs()
                                  .WithBucket(storageBucket).WithObject(product.ImagePath)
                                  .WithCallbackStream((stream) =>
                                  {
                                      stream.CopyTo(memoryStream);
                                  });
                        await minioClient.GetObjectAsync(getObjectArgs);
                        byte[] fileBytes = memoryStream.ToArray();
                        return File(fileBytes, stat.Type, stat.Name);
                    }
                }
                catch (Exception ex)
                {
                    return StatusCode(500, ex.Message);
                }
            }
            return NotFound();
        }

        private async Task<StockFileInfo> GetFileInfo(string filePath)
        {
            var storageURL = _config["Storage:URL"];
            var storageUsername = _config["Storage:Username"];
            var storagePassword = _config["Storage:Password"];
            var storageBucket = _config["Storage:Bucket"];
            var minioClient = new MinioClient().WithEndpoint(storageURL).WithCredentials(storageUsername, storagePassword).Build();
            var argsStat = new StatObjectArgs().WithBucket(storageBucket).WithObject(filePath);
            var stat = await minioClient.StatObjectAsync(argsStat);
            return new StockFileInfo
            {
                Name = stat.ObjectName,
                Type = stat.ContentType,
                Length = stat.Size
            };
        }
    }
}