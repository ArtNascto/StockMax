using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace StockMax.API
{
    public class RouteConvention : IControllerModelConvention
    {
        public void Apply(ControllerModel controller)
        {
            controller.Selectors[0].AttributeRouteModel = new AttributeRouteModel()
            {
                Template = $"api/{controller.Selectors[0].AttributeRouteModel.Template}"
            };
        }
    }
}