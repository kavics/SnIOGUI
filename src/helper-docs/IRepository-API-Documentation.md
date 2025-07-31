# SenseNet.Client IRepository API Documentation

## Overview
Az `IRepository` interface a SenseNet Client könyvtár fõ API-ja, amely lehetõvé teszi a sensenet repository-val való kommunikációt. Ez az interface definiálja a content menedzsment mûveleteket, lekérdezéseket, és egyéb repository specifikus funkciókat.

**Assembly:** SenseNet.Client, Version=4.0.0.0  
**Namespace:** SenseNet.Client

---

## Properties

### Server
```csharp
ServerContext Server { get; set; }
```
A sensenet szolgáltatáshoz való kapcsolatot reprezentáló context objektum. Egy repository instance mindig egyetlen sensenet szolgáltatáshoz tartozik.

### GlobalContentTypes
```csharp
RegisteredContentTypes GlobalContentTypes { get; }
```
A regisztrált repository-független content típusokat tartalmazza.

### Content
```csharp
ContentSet<Content> Content { get; }
```
A repository összes content-jének elérését biztosító property.

---

## Content Creation Methods

### CreateExistingContent
Meglévõ content in-memory reprezentációjának létrehozása a szerverrõl való betöltés nélkül.

```csharp
Content CreateExistingContent(int id)
Content CreateExistingContent(string path)
T CreateExistingContent<T>(int id) where T : Content
T CreateExistingContent<T>(string path) where T : Content
```

**Parameters:**
- `id`: Content azonosító
- `path`: Content elérési útvonal

**Exceptions:**
- `ArgumentException`: Ha az id <= 0
- `ArgumentNullException`: Ha a path null
- `ArgumentException`: Ha a path üres
- `ApplicationException`: Ha a kért típus nincs regisztrálva

### CreateContent
Új content instance létrehozása memóriában.

```csharp
Content CreateContent(string parentPath, string contentTypeName, string name)
T CreateContent<T>(string parentPath, string contentTypeName, string name) where T : Content
```

**Parameters:**
- `parentPath`: Már létezõ parent elérési útvonal (kötelezõ)
- `contentTypeName`: Content típus neve (kötelezõ)
- `name`: Content neve (opcionális)

### CreateContentByTemplate
Új content instance létrehozása content template alapján.

```csharp
Content CreateContentByTemplate(string parentPath, string contentTypeName, string name, string contentTemplate)
T CreateContentByTemplate<T>(string parentPath, string contentTypeName, string name, string contentTemplate) where T : Content
```

**Parameters:**
- `parentPath`: Parent elérési útvonal
- `contentTypeName`: Content típus neve
- `name`: Content neve (null esetén a szerver generál nevet)
- `contentTemplate`: Content template neve

---

## Content Loading Methods

### LoadContentAsync
Meglévõ content betöltése.

```csharp
Task<Content> LoadContentAsync(int id, CancellationToken cancel)
Task<Content> LoadContentAsync(string path, CancellationToken cancel)
Task<Content> LoadContentAsync(LoadContentRequest requestData, CancellationToken cancel)
Task<T> LoadContentAsync<T>(int id, CancellationToken cancel) where T : Content
Task<T> LoadContentAsync<T>(string path, CancellationToken cancel) where T : Content
Task<T> LoadContentAsync<T>(LoadContentRequest requestData, CancellationToken cancel) where T : Content
```

**Parameters:**
- `id`: Content id
- `path`: Content elérési útvonal
- `requestData`: Részletes request információ
- `cancel`: Cancellation token

**Returns:** Content vagy null Task-ba csomagolva

### LoadCollectionAsync
Gyermek elemek betöltése.

```csharp
Task<IContentCollection<Content>> LoadCollectionAsync(LoadCollectionRequest requestData, CancellationToken cancel)
Task<IContentCollection<T>> LoadCollectionAsync<T>(LoadCollectionRequest requestData, CancellationToken cancel) where T : Content
```

**Remarks:** Ez a metódus csak közvetlen gyermek elemeket tölt be, nem a teljes részfát.

### LoadReferenceAsync
Referencia mezõbõl hivatkozott content betöltése.

```csharp
Task<Content> LoadReferenceAsync(LoadReferenceRequest requestData, CancellationToken cancel)
Task<TContent> LoadReferenceAsync<TContent>(LoadReferenceRequest requestData, CancellationToken cancel) where TContent : Content
```

### LoadReferencesAsync
Multi-reference mezõbõl hivatkozott content-ek betöltése.

```csharp
Task<IContentCollection<Content>> LoadReferencesAsync(LoadReferenceRequest requestData, CancellationToken cancel)
Task<IContentCollection<TContent>> LoadReferencesAsync<TContent>(LoadReferenceRequest requestData, CancellationToken cancel) where TContent : Content
```

---

## Query Methods

### QueryAsync
Content elemek lekérdezése query alapján.

```csharp
Task<IContentCollection<Content>> QueryAsync(QueryContentRequest requestData, CancellationToken cancel)
Task<IContentCollection<T>> QueryAsync<T>(QueryContentRequest requestData, CancellationToken cancel) where T : Content
```

**Remarks:** Ez a metódus képes a teljes repository-ból content-eket betölteni, nem csak egyetlen mappából.

### QueryForAdminAsync
Content elemek lekérdezése életciklus és rendszer szûrõk kikapcsolásával.

```csharp
Task<IContentCollection<Content>> QueryForAdminAsync(QueryContentRequest requestData, CancellationToken cancel)
Task<IContentCollection<T>> QueryForAdminAsync<T>(QueryContentRequest requestData, CancellationToken cancel) where T : Content
```

### QueryCountAsync / QueryCountForAdminAsync
Content elemek számának lekérdezése.

```csharp
Task<int> QueryCountAsync(QueryContentRequest requestData, CancellationToken cancel)
Task<int> QueryCountForAdminAsync(QueryContentRequest requestData, CancellationToken cancel)
```

---

## Content Management Methods

### DeleteContentAsync
Content törlése.

```csharp
Task DeleteContentAsync(string path, bool permanent, CancellationToken cancel)
Task DeleteContentAsync(string[] paths, bool permanent, CancellationToken cancel)
Task DeleteContentAsync(int id, bool permanent, CancellationToken cancel)
Task DeleteContentAsync(int[] ids, bool permanent, CancellationToken cancel)
Task DeleteContentAsync(object[] idsOrPaths, bool permanent, CancellationToken cancel)
```

**Parameters:**
- `path/paths`: Content elérési útvonal(ak)
- `id/ids`: Content azonosító(k)
- `idsOrPaths`: Id-k vagy path-ok keverve
- `permanent`: Végleges törlés vagy Trash-be mozgatás

### IsContentExistsAsync
Content létezésének ellenõrzése.

```csharp
Task<bool> IsContentExistsAsync(string path, CancellationToken cancel)
```

**Returns:** True, ha a content létezik és az aktuális felhasználó hozzáfér

---

## File Operations

### UploadAsync
File feltöltés különbözõ módokkal.

```csharp
Task<UploadResult> UploadAsync(UploadRequest request, Stream stream, CancellationToken cancel)
Task<UploadResult> UploadAsync(UploadRequest request, Stream stream, Action<int> progressCallback, CancellationToken cancel)
Task<UploadResult> UploadAsync(UploadRequest request, string fileText, CancellationToken cancel)
```

**Parameters:**
- `request`: Feltöltési paraméterek
- `stream`: Feltöltendõ stream
- `fileText`: Szöveges file tartalom (kis fájlokhoz)
- `progressCallback`: Haladás callback (nagy fájlokhoz)

### DownloadAsync
Bináris stream letöltése.

```csharp
Task DownloadAsync(DownloadRequest request, Func<Stream, StreamProperties, Task> responseProcessor, CancellationToken cancel)
```

**Example:**
```csharp
string text;
var request = new DownloadRequest { ContentId = 142 };
await repository.DownloadAsync(request, async (stream, props) =>
{
    using var reader = new StreamReader(stream);
    text = await reader.ReadToEndAsync();
}, cancel);
```

### GetBlobToken
Blob storage token lekérése.

```csharp
Task<string> GetBlobToken(int id, CancellationToken cancel, string version = null, string propertyName = null)
Task<string> GetBlobToken(string path, CancellationToken cancel, string version = null, string propertyName = null)
```

---

## User Management

### GetCurrentUserAsync
Aktuális felhasználó lekérése.

```csharp
Task<Content> GetCurrentUserAsync(CancellationToken cancel)
Task<Content> GetCurrentUserAsync(string[] select, string[] expand, CancellationToken cancel)
```

---

## Operations (Actions & Functions)

### InvokeFunctionAsync
Szerver funkció hívása különbözõ visszatérési típusokkal.

```csharp
Task<T> InvokeFunctionAsync<T>(OperationRequest request, CancellationToken cancel)
Task<T> InvokeContentFunctionAsync<T>(OperationRequest request, CancellationToken cancel) where T : Content
Task<IContentCollection<T>> InvokeContentCollectionFunctionAsync<T>(OperationRequest request, CancellationToken cancel) where T : Content
```

### InvokeActionAsync
Szerver action végrehajtása.

```csharp
Task InvokeActionAsync(OperationRequest request, CancellationToken cancel)
Task<T> InvokeActionAsync<T>(OperationRequest request, CancellationToken cancel)
Task<T> InvokeContentActionAsync<T>(OperationRequest request, CancellationToken cancel) where T : Content
Task<IContentCollection<T>> InvokeContentCollectionActionAsync<T>(OperationRequest request, CancellationToken cancel) where T : Content
```

---

## Low-Level HTTP Methods

### GetResponseAsync
Generikus HTTP válasz lekérése.

```csharp
Task<T> GetResponseAsync<T>(ODataRequest requestData, HttpMethod method, CancellationToken cancel)
Task<dynamic> GetResponseJsonAsync(ODataRequest requestData, HttpMethod method, CancellationToken cancel)
Task<string> GetResponseStringAsync(ODataRequest requestData, HttpMethod method, CancellationToken cancel)
Task<string> GetResponseStringAsync(Uri uri, HttpMethod method, string postData, Dictionary<string, IEnumerable<string>> additionalHeaders, CancellationToken cancel)
```

### ProcessWebResponseAsync / ProcessWebRequestResponseAsync
HTTP request/response feldolgozás callback-ekkel.

```csharp
Task ProcessWebResponseAsync(string relativeUrl, HttpMethod method, Dictionary<string, IEnumerable<string>> additionalHeaders, HttpContent httpContent, Func<HttpResponseMessage, CancellationToken, Task> responseProcessor, CancellationToken cancel)

Task ProcessWebRequestResponseAsync(string relativeUrl, HttpMethod method, Dictionary<string, IEnumerable<string>> additionalHeaders, Action<HttpClientHandler, HttpClient, HttpRequestMessage> requestProcessor, Func<HttpResponseMessage, CancellationToken, Task> responseProcessor, CancellationToken cancel)
```

---

## Helper Methods

### GetContentTypeByName
Content típus lekérése név alapján.

```csharp
Type? GetContentTypeByName(string? contentTypeName)
```

### CreateContentFromJson
Content létrehozása JSON objektumból.

```csharp
Content CreateContentFromJson(JObject jObject, Type contentType = null)
```

### GetContentCountAsync
Gyermek collection számának lekérése.

```csharp
Task<int> GetContentCountAsync(LoadCollectionRequest requestData, CancellationToken cancel)
```

### ProcessOperationResponseAsync
Szerver mûvelet válaszának feldolgozása.

```csharp
Task ProcessOperationResponseAsync(OperationRequest request, HttpMethod method, Action<string> responseProcessor, CancellationToken cancel)
```

---

## Usage Examples

### Basic Content Loading
```csharp
// Content betöltése ID alapján
var content = await repository.LoadContentAsync(123, cancellationToken);

// Content betöltése path alapján
var rootContent = await repository.LoadContentAsync("/Root", cancellationToken);

// Típusos content betöltése
var file = await repository.LoadContentAsync<File>("/Root/MyFile.txt", cancellationToken);
```

### Content Query
```csharp
var request = new QueryContentRequest
{
    Query = "+TypeIs:Folder +InTree:/Root/Content"
};
var folders = await repository.QueryAsync(request, cancellationToken);
```

### Content Creation
```csharp
// Új folder létrehozása
var newFolder = repository.CreateContent("/Root/Content", "Folder", "MyFolder");
await newFolder.SaveAsync();

// Template alapú létrehozás
var docLib = repository.CreateContentByTemplate("/Root/Sites/MySite", "DocumentLibrary", "Documents", "DocumentLibrary");
await docLib.SaveAsync();
```

---

## Error Handling

**Common Exceptions:**
- `ClientException`: Érvénytelen request vagy szerver hiba
- `ArgumentException`: Érvénytelen paraméterek
- `ArgumentNullException`: Null paraméterek
- `ApplicationException`: Nem regisztrált típusok
- `InvalidCastException`: Típus konverziós hibák

**Best Practices:**
- Mindig használj `CancellationToken`-t hosszan futó mûveletekhez
- Ellenõrizd a content létezését `IsContentExistsAsync`-kel mentés elõtt
- Használj typed generics-et típusbiztonság érdekében
- Kezeld megfelelõen az async/await pattern-t

---

## Related Types

- `Content`: Alap content osztály
- `LoadContentRequest`: Content betöltési paraméterek
- `LoadCollectionRequest`: Collection betöltési paraméterek
- `QueryContentRequest`: Query paraméterek
- `OperationRequest`: Operation paraméterek
- `UploadRequest` / `DownloadRequest`: File mûveleti paraméterek
- `ServerContext`: Szerver kapcsolat context
- `IContentCollection<T>`: Content kollekció interface

---

*Ez a dokumentáció a SenseNet.Client 4.0.0 verzió alapján készült.*