using Cloth.Models;
using Cloth.Models.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Globalization;

namespace Cloth.Controllers
{
    public class ChartController : Controller
    {
        private DataContext context;
        private AnalyticsModel analyticsModel;

        public ChartController(DataContext ctx, AnalyticsModel mdl)
        {
            context = ctx;
            analyticsModel = mdl;
        }
        public IActionResult Chart()
        {

            var source = new AnalyticsModel();
            source.Products = context.Products.Select(a => new Products
            {
                Name = a.Name,
                Id = a.Id,
                CategoryId = a.CategoryId,
                BrandId = a.BrandId,
                Price = a.Price,
                OptionsId = a.OptionsId,
            });

            int MostBigPrice = 1;
            foreach (var q in context.Products.OrderBy(a => a.Price))
            {
                source.Max = q.Price;
                MostBigPrice = q.Price;
            }

            int MostSmallPrice = 1;
            foreach (var q in context.Products.OrderByDescending(q => q.Price))
            {
                source.Min = q.Price;
                MostSmallPrice = q.Price;
            }
            int SmallPriceFlag = MostSmallPrice;

            int ProductQuantity = MostBigPrice / MostSmallPrice;

            source.FirstPoint = MostBigPrice * 0.30;
            source.SecondPoint = MostBigPrice * 0.35;
            source.ThirdPoint = MostBigPrice * 0.65;
            source.FourthPoint = MostBigPrice * 0.70;

            for (int i = 0; i < ProductQuantity; i++)
            {
                source.QuantityPoint.Add(Convert.ToString(MostSmallPrice) + ",");
                source.MidlePrice.Add(Convert.ToString(1) + ",");
                source.TopPrice.Add(Convert.ToString(0.7, CultureInfo.InvariantCulture) + ", ");

                if (MostSmallPrice < source.FourthPoint + 1)
                {
                    source.First.Add(Convert.ToString((MostSmallPrice - SmallPriceFlag) / (source.FourthPoint - SmallPriceFlag), CultureInfo.InvariantCulture) + ",");
                }
                else
                {
                    source.First.Add(1 + ",");
                }


                if (MostSmallPrice > source.FirstPoint + 1)
                {
                    double var = (source.FirstPoint - MostSmallPrice) / (MostBigPrice - source.FirstPoint) + 1;
                    source.Second.Add(Convert.ToString(var, CultureInfo.InvariantCulture) + ","); ;
                }
                else
                {
                    source.Second.Add(1 + ",");
                }


                if (MostSmallPrice < source.SecondPoint)
                {
                    double var = (MostSmallPrice - SmallPriceFlag) / (source.SecondPoint - SmallPriceFlag);
                    source.Third.Add(Convert.ToString(var, CultureInfo.InvariantCulture) + ",");
                }
                else if (MostSmallPrice > source.ThirdPoint + 1)
                {
                    double var = (source.ThirdPoint - MostSmallPrice) / (MostBigPrice - source.ThirdPoint) + 1;
                    source.Third.Add(Convert.ToString(var, CultureInfo.InvariantCulture) + ",");
                }
                else
                {
                    source.Third.Add(1 + ",");
                }

                MostSmallPrice += SmallPriceFlag;
            }
            return View(source);
        } 

        
    }
}
