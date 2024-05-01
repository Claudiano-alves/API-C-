var builder = WebApplication.CreateBuilder(args);

//Configuração do banco de dados
builder.Services.AddDbContext<BancoDeDados>();

//Configuração Swagger
//http://localhost:porta/swagger/index.html
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI();

app.MapGet("/", () => "Escola API");

//APIs
app.MapPessoasApi();
app.MapProdutosApi();

app.Run();
