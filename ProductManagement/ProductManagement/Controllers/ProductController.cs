using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProductManagement.Configration;
using ProductManagement.Model;
using ProductManagement.RequestDto;
using ProductManagement.Services;
using System.Linq;

namespace ProductManagement.Controllers
{
    [Route("api/product")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly MyDbContext _context;
        private readonly IDbHelperSevices _dbHelperSevices;
        public ProductController(MyDbContext myDbContext, IDbHelperSevices dbHelperSevices) 
        {
            this._context = myDbContext;
            this._dbHelperSevices = dbHelperSevices;
        }

        [HttpGet]
        [Route("get")]
        public IActionResult GetProduct(string code)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(code))
                    throw new Exception("Product name is required.");

                var data = _dbHelperSevices.GetProducts().Where(x => string.IsNullOrEmpty(x.ProductCode) && x.ProductCode.Equals(code.Trim().ToLower()))?.ToList();
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex?.Message);
            }
        }

        [HttpPost]
        [Route("add")]
        public IActionResult AddProduct(ProductRequestDto  request)
        {
            try
            {
                var userId = HttpContext.GetUserId();

                if (string.IsNullOrWhiteSpace(request.ProductCode))
                    throw new Exception("Product name is required.");

                if(_dbHelperSevices.GetProducts().Any(x=>string.IsNullOrEmpty(x.ProductCode) && x.ProductCode.Equals(request.ProductCode.Trim().ToLower())))
                    throw new Exception("Product name is already exist.");

                var insertdata = new Product()
                {
                    ProductCode = request.ProductCode.Trim(),
                    Identifier = Guid.NewGuid(),
                    CratedDate = DateTime.UtcNow,
                    UpdateDate = DateTime.UtcNow,
                    Price = request.Price
                };

                _dbHelperSevices.AddProducts(insertdata);

                return Ok("Product added successfully");
            }
            catch (Exception ex)
            {
                return BadRequest(ex?.Message);
            }
        }

        [HttpPost]
        [Route("update")]
        public IActionResult UpdateProduct(ProductRequestDto request)
        {
            try
            {
                if (String.IsNullOrWhiteSpace(request.ProductCode))
                    throw new Exception("Product id is invalid.");

                var userId = HttpContext.GetUserId();

                var product = _dbHelperSevices.GetProducts(false).Where(x => !string.IsNullOrEmpty(x.ProductCode) && x.ProductCode.ToLower().Equals(request.ProductCode.ToLower().Trim()))?.FirstOrDefault();

                if (product is null)
                    throw new Exception("Product is not exist.");


                product.Price = request.Price;
                product.UpdateDate = DateTime.UtcNow;
                _dbHelperSevices.UpdateProduct(product);

                return Ok("Product updated successfully");
            }
            catch (Exception ex)
            {
                return BadRequest(ex?.Message);
            }
        }

        [HttpPost]
        [UserAuthActionFilter]
        [Route("user/custom/price/update")]
        public IActionResult UpdateUserCustomProduct(ProductRequestDto request)
        {
            try
            {
                if (String.IsNullOrWhiteSpace(request.ProductCode))
                    throw new Exception("Product id is invalid.");

                var userId = HttpContext.GetUserId();

                var product = _dbHelperSevices.GetProducts().Where(x => !string.IsNullOrEmpty(x.ProductCode) && x.ProductCode.ToLower().Equals(request.ProductCode.ToLower().Trim()))?.FirstOrDefault();
              
                if (product is null)
                    throw new Exception("Product is not exist.");

                var specificUserProductPrices = _dbHelperSevices.GetProductCustomPrice().Where(x => x.Id == userId && x.ProductId == product.Id)?.FirstOrDefault();
                if(specificUserProductPrices is null)
                {
                    specificUserProductPrices = new ProductCustomPriceMap()
                    {
                        ProductId = product.Id,
                        CustomerId = userId,
                        Price = request.Price
                    };
                    _dbHelperSevices.AddProductCustom(specificUserProductPrices);
                }
                else
                {
                    specificUserProductPrices.Price = request.Price;
                    _dbHelperSevices.UpdateProductCustom(specificUserProductPrices);
                }
              
                return Ok("Product updated successfully");
            }
            catch (Exception ex)
            {
                return BadRequest(ex?.Message);
            }
        }

        [HttpDelete]
        [Route("{productCode}/delete")]
        public IActionResult DeleteProduct(string productCode)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(productCode))
                    throw new Exception("Product guid is invalid.");

                var deleteProduct = _dbHelperSevices.GetProducts(false).Where(x => !string.IsNullOrEmpty(x.ProductCode) && x.ProductCode.ToLower().Equals(productCode.Trim().ToLower()))?.FirstOrDefault();
                
                if(deleteProduct is null)
                    throw new Exception("Product is not exist.");

                _dbHelperSevices.DeleteProduct(deleteProduct);
                return Ok("Product deleted successfully");
            }
            catch (Exception ex)
            {
                return BadRequest(ex?.Message);
            }
        }
    }
}
