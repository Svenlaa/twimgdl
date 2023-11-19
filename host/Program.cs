using twimgdl_host;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy => {
        policy.WithOrigins("https://twitter.com").WithMethods("GET");
    });
});

var app = builder.Build();
app.UseCors();

const string FOLDER_URL = "../images";

static Status SaveTwitterImage(string imageId)
{
    HttpClient client = new();
    HttpResponseMessage response = client.GetAsync($"https://pbs.twimg.com/media/{imageId}?format=jpg").Result;
    if (!response.IsSuccessStatusCode) return Status.NotFound;
    DateTimeOffset lastModified = response.Content.Headers.LastModified ?? DateTimeOffset.Now;
    Stream stream = response.Content.ReadAsStreamAsync().Result;

    Directory.CreateDirectory(FOLDER_URL);

    string filename = $"{FOLDER_URL}/{imageId}.jpg";
    if (File.Exists(filename)) return Status.Conflict;

    using var fileStream = File.Create(filename);
    stream.CopyToAsync(fileStream).Wait();
    File.SetLastWriteTime(filename, lastModified.ToLocalTime().DateTime);

    return Status.Ok;
}

app.MapGet("/status", () => true);

app.MapGet("/image/{imageId}", (string imageId) =>
{
    return SaveTwitterImage(imageId) switch
    {
        Status.Ok => Results.Ok(),
        Status.NotFound => Results.NotFound(),
        Status.Conflict => Results.Conflict(),
        _ => Results.BadRequest(),
    };
});

app.Run();
