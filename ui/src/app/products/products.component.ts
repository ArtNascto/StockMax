import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { NgxSpinnerModule, NgxSpinnerService } from 'ngx-spinner';
import { ConfirmationService, MessageService, SelectItem } from 'primeng/api';
import { ButtonModule } from 'primeng/button';
import { CardModule } from 'primeng/card';
import { CheckboxModule } from 'primeng/checkbox';
import { InputTextModule } from 'primeng/inputtext';
import { PasswordModule } from 'primeng/password';
import { Color, Product } from '../../services/models/product.model';
import { TableModule } from 'primeng/table';
import { CommonModule } from '@angular/common';
import { DialogModule } from 'primeng/dialog';
import { ImageModule } from 'primeng/image';
import { FileUploadModule } from 'primeng/fileupload';
import { FloatLabelModule } from 'primeng/floatlabel';
import { v4 as uuidv4 } from 'uuid';
import { DomSanitizer, SafeResourceUrl } from '@angular/platform-browser';
import { InputMaskModule } from 'primeng/inputmask';
import { CurrencyMaskModule } from 'ng2-currency-mask';
import { InputNumberModule } from 'primeng/inputnumber';
import { MultiSelectModule } from 'primeng/multiselect';
import { DropdownModule } from 'primeng/dropdown';
import { EditorModule } from 'primeng/editor';
import { SplitButtonModule } from 'primeng/splitbutton';
import { MatMenuModule } from '@angular/material/menu';
import { MessagesModule } from 'primeng/messages';
import { ToastModule } from 'primeng/toast';
import { RippleModule } from 'primeng/ripple';
import { QuillModule } from 'ngx-quill';
import { ConfirmDialogModule } from 'primeng/confirmdialog';

@Component({
  selector: 'app-products',
  standalone: true,
  imports: [
    ConfirmDialogModule,
    QuillModule,
    QuillModule,
    RippleModule,
    ToastModule,
    MessagesModule,
    MatMenuModule,
    SplitButtonModule,
    EditorModule,
    DropdownModule,
    MultiSelectModule,
    CurrencyMaskModule,
    InputNumberModule,
    InputMaskModule,
    FloatLabelModule,
    FileUploadModule,
    ImageModule,
    CommonModule,
    TableModule,
    CardModule,
    NgxSpinnerModule,
    ButtonModule,
    CheckboxModule,
    InputTextModule,
    PasswordModule,
    MatCardModule,
    MatInputModule,
    MatButtonModule,
    MatFormFieldModule,
    DialogModule,
    FormsModule,
    ReactiveFormsModule,
  ],
  templateUrl: './products.component.html',
  styleUrl: './products.component.scss',
})
export class ProductsComponent implements OnInit {
  apiEndpoint: string = '';
  products: Array<Product> = [];
  statuses: SelectItem[] = [
    { value: 'em_estoque', label: 'Em estoque' },
    { value: 'fora_de_estoque', label: 'Fora de estoque' },
    { value: 'promoção', label: 'Promoção' },
  ];
  newProduct: Product = {};
  product?: Product;
  first = 0;
  rows = 10;
  showCreateProduct = false;
  showImage = false;
  defaultProductImage: string = 'assets/image/product.png';
  productImage?: SafeResourceUrl;
  categoriesLst: SelectItem[] = [];
  columns: any[] = [
    {
      name: 'Código',
      field: 'code',
      type: 'string',
      orderBy: true,
    },
    {
      name: 'Nome',
      field: 'name',
      type: 'string',
      orderBy: true,
    },
    {
      name: 'Categoria',
      field: 'category',
      type: 'string',
      orderBy: false,
    },
    {
      name: 'Quantidade',
      field: 'quantity',
      type: 'string',
      orderBy: false,
    },
    {
      name: 'Data Cadastro',
      field: 'creationTime',
      type: 'date',
      orderBy: true,
    },
  ];
  totalRecords = 0;
  categories: Array<string> = [
    'Almofadas Circulares em tecido Velboa',
    'Almofadas Retangulares em tecido Velboa',
    'Almofadas Diabinho em tecido Velboa',
    'Almofadas Emoji em tecido Velboa',
    'Almofada Fantasminha e Monstrinho em tecido Velboa',
    'Almofadas Sublimadas',
    'Azulejos Sublimados',
    'Bonecas e Bonecos de Pano',
    'Bonecas sob encomenda',
    'Bonecos Temáticos (Páscoa, Natal, Festa Junina)',
    'Papai Noel Cortineiro',
    'Papai Noel Porta Talher',
    'Bonecos de Neve - modelos 1, 2 e 3',
    'Naninha Raposinha',
    'Porta Celular',
    'Cartonagem',
    'Kit apagador',
    'Caixa Livro',
    'Bomboniere',
    'Cachepó',
    'Maleta',
    'Caixa Utilitária',
    'Prancheta',
    'Caixa Caneca',
    'Produtos Sublimáveis',
    'Canecas de Porcelana',
    'Caneca Cônica',
    'Squeeze',
    'Canecas de Vidro',
    'Caneca Chopp Vidro Fosco por fora',
    'Caneca de Vidro Fosco',
    'Camisas e Camisetas',
    'Outros Produtos Sublimáveis',
    'Lixinho para carro',
    'Clips para costura',
    'Rebites',
    'Produtos em MDF e ACRÍLICO',
    'Sacolas de TNT sublimáveis',
    'Bordados',
  ];
  newColorOption: string = '';
  colors: Color[] = [];
  globalFilter: string = '';
  constructor(
    private confirmationService: ConfirmationService,
    private spinner: NgxSpinnerService,
    private http: HttpClient,
    private messageService: MessageService,
    private sanitizer: DomSanitizer
  ) {
    this.apiEndpoint = 'http://localhost:8080/api';
    this.categories.forEach((c) => {
      this.categoriesLst.push({ value: c, label: c });
    });
  }
  paginate(event: any) {
    this.first = event.first;
    this.loadProducts(event);
  }
  next() {
    this.first = this.first + this.rows;
  }

  reset() {
    this.first = 0;
  }

  isLastPage(): boolean {
    return this.totalRecords
      ? this.first === this.totalRecords - this.rows
      : true;
  }

  isFirstPage(): boolean {
    return this.products ? this.first === 0 : true;
  }
  prev() {
    this.first = this.first - this.rows;
  }
  edit(productId: string) {
    this.spinner.show();
    this.http
      .get<Product>(`${this.apiEndpoint}/product/get/${productId}`)
      .subscribe(
        (data) => {
          this.spinner.hide();
          if (data && data != undefined) {
            this.newProduct = {
              category: data.category,
              Category: data.category,
              Code: data.code,
              code: data.code,
              Colors: data.colors,
              colors: data.colors,
              colorsArr: data.colors?.split(','),
              ColorsArr: data.colors?.split(','),
              CreationTime: data.creationTime,
              creationTime: data.creationTime,
              DeletionTime: data.deletionTime,
              deletionTime: data.deletionTime,
              Description: data.description + '',
              description: data.description + '',
              Id: data.id,
              id: data.id,
              ImagePath: data.imagePath,
              imagePath: data.imagePath,
              Label: data.label,
              label: data.label,
              LastUpdate: data.lastUpdate,
              lastUpdate: data.lastUpdate,
              Name: data.name,
              name: data.name,
              Quantity: data.quantity,
              quantity: data.quantity,
              Status: data.status,
              status: data.status,
              Value: data.value,
              value: data.value,
              Vendor: data.vendor,
              vendor: data.vendor,
            };
            if (
              this.newProduct.imagePath &&
              this.newProduct.imagePath != '' &&
              this.newProduct.imagePath != undefined
            ) {
              this.productImage = this.safeURL(
                this.apiEndpoint +
                  '/product/getProductImage/' +
                  this.newProduct.id
              );
            } else {
              this.productImage = this.safeURL(this.defaultProductImage);
            }
            this.showImage = true;
            this.showCreateProduct = true;
          }
        },
        (err) => {
          this.spinner.hide();
          console.log({ err });
          this.messageService.add({
            severity: 'danger',
            summary: 'Erro!',
            detail: 'Erro ao remover produto!',
          });
        }
      );
  }

  onSearch() {
    this.spinner.show();
    this.first = 0; // reset to the first page
    this.loadProducts();
  }
  delete(productId: string, event: any) {
    this.spinner.show();
    this.http
      .get<Product>(`${this.apiEndpoint}/product/get/${productId}`)
      .subscribe(
        (data) => {
          this.spinner.hide();
          if (data && data != undefined) {
            this.newProduct = {
              category: data.category,
              Category: data.category,
              Code: data.code,
              code: data.code,
              Colors: data.colors,
              colors: data.colors,
              colorsArr: data.colors?.split(','),
              ColorsArr: data.colors?.split(','),
              CreationTime: data.creationTime,
              creationTime: data.creationTime,
              DeletionTime: data.deletionTime,
              deletionTime: data.deletionTime,
              Description: data.description + '',
              description: data.description + '',
              Id: data.id,
              id: data.id,
              ImagePath: data.imagePath,
              imagePath: data.imagePath,
              Label: data.label,
              label: data.label,
              LastUpdate: data.lastUpdate,
              lastUpdate: data.lastUpdate,
              Name: data.name,
              name: data.name,
              Quantity: data.quantity,
              quantity: data.quantity,
              Status: data.status,
              status: data.status,
              Value: data.value,
              value: data.value,
              Vendor: data.vendor,
              vendor: data.vendor,
            };
            this.confirmationService.confirm({
              target: event.target as EventTarget,
              message: `Você gostaria de remover o produto "${this.newProduct.code} - ${this.newProduct.name}"`,
              header: 'Confirmação de deleção',
              icon: 'pi pi-info-circle',
              acceptButtonStyleClass: 'p-button-text p-button-text',
              rejectButtonStyleClass: 'p-button-danger p-button-text',
              acceptIcon: 'none',
              rejectIcon: 'none',
              acceptLabel: 'Sim',
              rejectLabel: 'Não',

              accept: () => {
                this.spinner.show();
                this.http
                  .delete(`${this.apiEndpoint}/product/delete/${productId}`)
                  .subscribe(
                    (r) => {
                      this.messageService.add({
                        severity: 'success',
                        summary: 'Sucesso!',
                        styleClass: 'bg-green-500 text-white',
                        detail: 'Produto removido com sucesso!',
                      });
                      this.spinner.show();
                      this.first = 0; // reset to the first page
                      this.loadProducts();
                    },
                    (err) => {
                      this.spinner.hide();
                      console.log({ err });
                      this.messageService.add({
                        severity: 'danger',
                        styleClass: 'bg-red-500 text-white',
                        summary: 'Erro!',
                        detail: 'Erro ao remover produto!',
                      });
                    }
                  );
              },
              reject: () => {},
            });
          }
        },
        (err) => {
          this.spinner.hide();
          console.log({ err });
          this.messageService.add({
            severity: 'danger',
            summary: 'Erro!',
            detail: 'Erro ao remover produto!',
          });
        }
      );
  }
  onColorChange(event: any) {}
  pageChange(event: any) {
    this.first = event.first;
    this.rows = event.rows;
  }
  loadProducts(event?: any) {
    this.spinner.show();
    let sort = 'code';
    let sortOrder = 1;
    if (event) {
      this.first = event.first;
      this.rows = event.rows;
      if (event.sortField && event.sortField != undefined) {
        sort = event.sortField;
        sortOrder = event.sortOrder;
      }
    }
    let pageIndex = this.first / this.rows;
    let params = {
      Page: pageIndex + 1,
      PageCount: this.rows,
      Query: this.globalFilter,
      OrderBy: sort + ' ' + (sortOrder > 0 ? 'desc' : 'asc'),
    };
    this.http
      .get<GetProductsOutput>(`${this.apiEndpoint}/product/getall`, { params })
      .subscribe(
        (data) => {
          this.products = [];
          if (data.value.length > 0) {
            data.value.forEach((v) => {
              this.products.push({
                category: v.Category,
                Category: v.Category,
                Code: v.Code,
                code: v.Code,
                Colors: v.Colors,
                colors: v.Colors,
                colorsArr: v.Colors?.split(','),
                ColorsArr: v.Colors?.split(','),
                CreationTime: v.CreationTime,
                creationTime: v.CreationTime,
                DeletionTime: v.DeletionTime,
                deletionTime: v.DeletionTime,
                Description: v.Description + '',
                description: v.Description + '',
                Id: v.Id,
                id: v.Id,
                ImagePath: v.ImagePath,
                imagePath: v.ImagePath,
                Label: v.Label,
                label: v.Label,
                LastUpdate: v.LastUpdate,
                lastUpdate: v.LastUpdate,
                Name: v.Name,
                name: v.Name,
                Quantity: v.Quantity,
                quantity: v.Quantity,
                Status: v.Status,
                status: v.Status,
                Value: v.Value,
                value: v.Value,
                Vendor: v.Vendor,
                vendor: v.Vendor,
              });
            });
          }
          this.totalRecords = data.totalCount;
          this.spinner.hide();
        },
        (err) => {
          this.spinner.hide();
          this.products = [];
        }
      );
  }
  addNewColorOption() {
    if (
      this.newColorOption &&
      !this.colors.some((o) => o.Value === this.newColorOption)
    ) {
      this.colors.push({
        Value: this.newColorOption,
      });
      this.newColorOption = '';
    }
  }
  close() {
    this.showCreateProduct = false;
    this.showImage = false;
  }
  getColors() {
    this.http
      .get<Color[]>(`${this.apiEndpoint}/product/getColors`)
      .subscribe((r) => {
        if (r && r.length > 0) {
          this.colors = r;
        }
      });
  }
  ngOnInit(): void {
    this.getColors();
    this.loadProducts();
  }
  save() {
    if (this.newProduct) {
      if (this.newProduct.colorsArr && this.newProduct.colorsArr?.length > 0) {
        this.newProduct.colors = this.newProduct.colorsArr.join(',');
      }
      this.http
        .post(this.apiEndpoint + '/product/create', this.newProduct)
        .subscribe((r) => {
          this.first = 0;
          this.close();
          this.messageService.add({
            severity: 'success',
            summary: 'Sucesso!',
            detail: 'Produto armazenado com sucesso!',
          });
          this.loadProducts();
        });
    }
  }
  add() {
    this.newProduct.id = uuidv4();
    this.productImage = this.safeURL(this.defaultProductImage);
    this.showImage = true;
    this.showCreateProduct = true;
  }
  onBasicUploadAuto(event: any) {
    this.showImage = false;
    this.productImage = this.safeURL(
      this.apiEndpoint +
        '/product/getProductImage/' +
        event.originalEvent.body.id
    );
    this.showImage = true;
  }
  safeURL(url: string): SafeResourceUrl {
    return this.sanitizer.bypassSecurityTrustResourceUrl(url);
  }
}
export interface GetProductsOutput {
  value: Array<Product>;
  totalCount: number;
  pageSize: number;
  currentPage: number;
  totalPages: number;
}
