namespace eCommerce.BackEndAPI
{
    public static class WebHostEnvironmentHelper
    {
        public static string GetWebRootPath()
        {
            var accessor = new HttpContextAccessor();
            return accessor.HttpContext.RequestServices.GetRequiredService<IWebHostEnvironment>().WebRootPath;
        }
    }
}
