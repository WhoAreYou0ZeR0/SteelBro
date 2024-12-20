namespace SteelBro.Middlewear
{
    public class CspMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IConfiguration _configuration;

        public CspMiddleware(RequestDelegate next, IConfiguration configuration)
        {
            _next = next;
            _configuration = configuration;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Генерация уникального nonce для страницы
            var nonce = Convert.ToBase64String(Guid.NewGuid().ToByteArray());

            // Добавляем nonce в контекст, чтобы передавать его в Razor страницы
            context.Items["CspNonce"] = nonce;

            // Устанавливаем заголовок CSP с новым nonce
            var cspHeader = _configuration["CspHeader"] ?? "default-src 'self'; script-src 'self' 'nonce-{nonce}';";
            cspHeader = cspHeader.Replace("{nonce}", nonce);

            context.Response.Headers.Add("Content-Security-Policy", cspHeader);

            // Продолжаем обработку запроса
            await _next(context);
        }
    }
}