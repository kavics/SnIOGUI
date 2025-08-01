# SenseNet.Client IRepository API Documentation

## Overview
Az `IRepository` interface a SenseNet Client k�nyvt�r f� API-ja, amely lehet�v� teszi a sensenet repository-val val� kommunik�ci�t. Ez az interface defini�lja a content menedzsment m�veleteket, lek�rdez�seket, �s egy�b repository specifikus funkci�kat.

**Assembly:** SenseNet.Client, Version=4.0.0.0  
**Namespace:** SenseNet.Client

---

## Properties

### Server
```csharp
ServerContext Server { get; set; }
```
A sensenet szolg�ltat�shoz val� kapcsolatot reprezent�l� context objektum. Egy repository instance mindig egyetlen sensenet szolg�ltat�shoz tartozik.

### GlobalContentTypes
```csharp
RegisteredContentTypes GlobalContentTypes { get; }
```
A regisztr�lt repository-f�ggetlen content t�pusokat tartalmazza.

### Content
```csharp
ContentSet<Content> Content { get; }
```
A repository �sszes content-j�nek el�r�s�t biztos�t� property.

---

## Content Creation Methods

### CreateExistingContent
Megl�v� content in-memory reprezent�ci�j�nak l�trehoz�sa a szerverr�l val� bet�lt�s n�lk�l.

```csharp
Content CreateExistingContent(int id)
Content CreateExistingContent(string path)
T CreateExistingContent<T>(int id) where T : Content
T CreateExistingContent<T>(string path) where T : Content
```

**Parameters:**
- `id`: Content azonos�t�
- `path`: Content el�r�si �tvonal

**Exceptions:**
- `ArgumentException`: Ha az id <= 0
- `ArgumentNullException`: Ha a path null
- `ArgumentException`: Ha a path �res
- `ApplicationException`: Ha a k�rt t�pus nincs regisztr�lva

### CreateContent
�j content instance l�trehoz�sa mem�ri�ban.

```csharp
Content CreateContent(string parentPath, string contentTypeName, string name)
T CreateContent<T>(string parentPath, string contentTypeName, string name) where T : Content
```

**Parameters:**
- `parentPath`: M�r l�tez� parent el�r�si �tvonal (k�telez�)
- `contentTypeName`: Content t�pus neve (k�telez�)
- `name`: Content neve (opcion�lis)

### CreateContentByTemplate
�j content instance l�trehoz�sa content template alapj�n.

```csharp
Content CreateContentByTemplate(string parentPath, string contentTypeName, string name, string contentTemplate)
T CreateContentByTemplate<T>(string parentPath, string contentTypeName, string name, string contentTemplate) where T : Content
```

**Parameters:**
- `parentPath`: Parent el�r�si �tvonal
- `contentTypeName`: Content t�pus neve
- `name`: Content neve (null eset�n a szerver gener�l nevet)
- `contentTemplate`: Content template neve

---

## Content Loading Methods

### LoadContentAsync
Megl�v� content bet�lt�se.

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
- `path`: Content el�r�si �tvonal
- `requestData`: R�szletes request inform�ci�
- `cancel`: Cancellation token

**Returns:** Content vagy null Task-ba csomagolva

### LoadCollectionAsync
Gyermek elemek bet�lt�se.

```csharp
Task<IContentCollection<Content>> LoadCollectionAsync(LoadCollectionRequest requestData, CancellationToken cancel)
Task<IContentCollection<T>> LoadCollectionAsync<T>(LoadCollectionRequest requestData, CancellationToken cancel) where T : Content
```

**Remarks:** Ez a met�dus csak k�zvetlen gyermek elemeket t�lt be, nem a teljes r�szf�t.

### LoadReferenceAsync
Referencia mez�b�l hivatkozott content bet�lt�se.

```csharp
Task<Content> LoadReferenceAsync(LoadReferenceRequest requestData, CancellationToken cancel)
Task<TContent> LoadReferenceAsync<TContent>(LoadReferenceRequest requestData, CancellationToken cancel) where TContent : Content
```

### LoadReferencesAsync
Multi-reference mez�b�l hivatkozott content-ek bet�lt�se.

```csharp
Task<IContentCollection<Content>> LoadReferencesAsync(LoadReferenceRequest requestData, CancellationToken cancel)
Task<IContentCollection<TContent>> LoadReferencesAsync<TContent>(LoadReferenceRequest requestData, CancellationToken cancel) where TContent : Content
```

---

## Query Methods

### QueryAsync
Content elemek lek�rdez�se query alapj�n.

```csharp
Task<IContentCollection<Content>> QueryAsync(QueryContentRequest requestData, CancellationToken cancel)
Task<IContentCollection<T>> QueryAsync<T>(QueryContentRequest requestData, CancellationToken cancel) where T : Content
```

**Remarks:** Ez a met�dus k�pes a teljes repository-b�l content-eket bet�lteni, nem csak egyetlen mapp�b�l.

### QueryForAdminAsync
Content elemek lek�rdez�se �letciklus �s rendszer sz�r�k kikapcsol�s�val.

```csharp
Task<IContentCollection<Content>> QueryForAdminAsync(QueryContentRequest requestData, CancellationToken cancel)
Task<IContentCollection<T>> QueryForAdminAsync<T>(QueryContentRequest requestData, CancellationToken cancel) where T : Content
```

### QueryCountAsync / QueryCountForAdminAsync
Content elemek sz�m�nak lek�rdez�se.

```csharp
Task<int> QueryCountAsync(QueryContentRequest requestData, CancellationToken cancel)
Task<int> QueryCountForAdminAsync(QueryContentRequest requestData, CancellationToken cancel)
```

---

## Content Management Methods

### DeleteContentAsync
Content t�rl�se.

```csharp
Task DeleteContentAsync(string path, bool permanent, CancellationToken cancel)
Task DeleteContentAsync(string[] paths, bool permanent, CancellationToken cancel)
Task DeleteContentAsync(int id, bool permanent, CancellationToken cancel)
Task DeleteContentAsync(int[] ids, bool permanent, CancellationToken cancel)
Task DeleteContentAsync(object[] idsOrPaths, bool permanent, CancellationToken cancel)
```

**Parameters:**
- `path/paths`: Content el�r�si �tvonal(ak)
- `id/ids`: Content azonos�t�(k)
- `idsOrPaths`: Id-k vagy path-ok keverve
- `permanent`: V�gleges t�rl�s vagy Trash-be mozgat�s

### IsContentExistsAsync
Content l�tez�s�nek ellen�rz�se.

```csharp
Task<bool> IsContentExistsAsync(string path, CancellationToken cancel)
```

**Returns:** True, ha a content l�tezik �s az aktu�lis felhaszn�l� hozz�f�r

---

## File Operations

### UploadAsync
File felt�lt�s k�l�nb�z� m�dokkal.

```csharp
Task<UploadResult> UploadAsync(UploadRequest request, Stream stream, CancellationToken cancel)
Task<UploadResult> UploadAsync(UploadRequest request, Stream stream, Action<int> progressCallback, CancellationToken cancel)
Task<UploadResult> UploadAsync(UploadRequest request, string fileText, CancellationToken cancel)
```

**Parameters:**
- `request`: Felt�lt�si param�terek
- `stream`: Felt�ltend� stream
- `fileText`: Sz�veges file tartalom (kis f�jlokhoz)
- `progressCallback`: Halad�s callback (nagy f�jlokhoz)

### DownloadAsync
Bin�ris stream let�lt�se.

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
Blob storage token lek�r�se.

```csharp
Task<string> GetBlobToken(int id, CancellationToken cancel, string version = null, string propertyName = null)
Task<string> GetBlobToken(string path, CancellationToken cancel, string version = null, string propertyName = null)
```

---

## User Management

### GetCurrentUserAsync
Aktu�lis felhaszn�l� lek�r�se.

```csharp
Task<Content> GetCurrentUserAsync(CancellationToken cancel)
Task<Content> GetCurrentUserAsync(string[] select, string[] expand, CancellationToken cancel)
```

---

## Operations (Actions & Functions)

### InvokeFunctionAsync
Szerver funkci� h�v�sa k�l�nb�z� visszat�r�si t�pusokkal.

```csharp
Task<T> InvokeFunctionAsync<T>(OperationRequest request, CancellationToken cancel)
Task<T> InvokeContentFunctionAsync<T>(OperationRequest request, CancellationToken cancel) where T : Content
Task<IContentCollection<T>> InvokeContentCollectionFunctionAsync<T>(OperationRequest request, CancellationToken cancel) where T : Content
```

### InvokeActionAsync
Szerver action v�grehajt�sa.

```csharp
Task InvokeActionAsync(OperationRequest request, CancellationToken cancel)
Task<T> InvokeActionAsync<T>(OperationRequest request, CancellationToken cancel)
Task<T> InvokeContentActionAsync<T>(OperationRequest request, CancellationToken cancel) where T : Content
Task<IContentCollection<T>> InvokeContentCollectionActionAsync<T>(OperationRequest request, CancellationToken cancel) where T : Content
```

---

## Low-Level HTTP Methods

### GetResponseAsync
Generikus HTTP v�lasz lek�r�se.

```csharp
Task<T> GetResponseAsync<T>(ODataRequest requestData, HttpMethod method, CancellationToken cancel)
Task<dynamic> GetResponseJsonAsync(ODataRequest requestData, HttpMethod method, CancellationToken cancel)
Task<string> GetResponseStringAsync(ODataRequest requestData, HttpMethod method, CancellationToken cancel)
Task<string> GetResponseStringAsync(Uri uri, HttpMethod method, string postData, Dictionary<string, IEnumerable<string>> additionalHeaders, CancellationToken cancel)
```

### ProcessWebResponseAsync / ProcessWebRequestResponseAsync
HTTP request/response feldolgoz�s callback-ekkel.

```csharp
Task ProcessWebResponseAsync(string relativeUrl, HttpMethod method, Dictionary<string, IEnumerable<string>> additionalHeaders, HttpContent httpContent, Func<HttpResponseMessage, CancellationToken, Task> responseProcessor, CancellationToken cancel)

Task ProcessWebRequestResponseAsync(string relativeUrl, HttpMethod method, Dictionary<string, IEnumerable<string>> additionalHeaders, Action<HttpClientHandler, HttpClient, HttpRequestMessage> requestProcessor, Func<HttpResponseMessage, CancellationToken, Task> responseProcessor, CancellationToken cancel)
```

---

## Helper Methods

### GetContentTypeByName
Content t�pus lek�r�se n�v alapj�n.

```csharp
Type? GetContentTypeByName(string? contentTypeName)
```

### CreateContentFromJson
Content l�trehoz�sa JSON objektumb�l.

```csharp
Content CreateContentFromJson(JObject jObject, Type contentType = null)
```

### GetContentCountAsync
Gyermek collection sz�m�nak lek�r�se.

```csharp
Task<int> GetContentCountAsync(LoadCollectionRequest requestData, CancellationToken cancel)
```

### ProcessOperationResponseAsync
Szerver m�velet v�lasz�nak feldolgoz�sa.

```csharp
Task ProcessOperationResponseAsync(OperationRequest request, HttpMethod method, Action<string> responseProcessor, CancellationToken cancel)
```

---

## Usage Examples

### Basic Content Loading
```csharp
// Content bet�lt�se ID alapj�n
var content = await repository.LoadContentAsync(123, cancellationToken);

// Content bet�lt�se path alapj�n
var rootContent = await repository.LoadContentAsync("/Root", cancellationToken);

// T�pusos content bet�lt�se
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
// �j folder l�trehoz�sa
var newFolder = repository.CreateContent("/Root/Content", "Folder", "MyFolder");
await newFolder.SaveAsync();

// Template alap� l�trehoz�s
var docLib = repository.CreateContentByTemplate("/Root/Sites/MySite", "DocumentLibrary", "Documents", "DocumentLibrary");
await docLib.SaveAsync();
```

---

## Error Handling

**Common Exceptions:**
- `ClientException`: �rv�nytelen request vagy szerver hiba
- `ArgumentException`: �rv�nytelen param�terek
- `ArgumentNullException`: Null param�terek
- `ApplicationException`: Nem regisztr�lt t�pusok
- `InvalidCastException`: T�pus konverzi�s hib�k

**Best Practices:**
- Mindig haszn�lj `CancellationToken`-t hosszan fut� m�veletekhez
- Ellen�rizd a content l�tez�s�t `IsContentExistsAsync`-kel ment�s el�tt
- Haszn�lj typed generics-et t�pusbiztons�g �rdek�ben
- Kezeld megfelel�en az async/await pattern-t

---

## Related Types

- `Content`: Alap content oszt�ly
- `LoadContentRequest`: Content bet�lt�si param�terek
- `LoadCollectionRequest`: Collection bet�lt�si param�terek
- `QueryContentRequest`: Query param�terek
- `OperationRequest`: Operation param�terek
- `UploadRequest` / `DownloadRequest`: File m�veleti param�terek
- `ServerContext`: Szerver kapcsolat context
- `IContentCollection<T>`: Content kollekci� interface

---

*Ez a dokument�ci� a SenseNet.Client 4.0.0 verzi� alapj�n k�sz�lt.*