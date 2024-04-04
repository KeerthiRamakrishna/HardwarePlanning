
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Web;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;
using Radzen;

namespace Osporting.Client
{
    public partial class OSPortDBService
    {
        private readonly HttpClient httpClient;
        private readonly Uri baseUri;
        private readonly NavigationManager navigationManager;

        public OSPortDBService(NavigationManager navigationManager, HttpClient httpClient, IConfiguration configuration)
        {
            this.httpClient = httpClient;

            this.navigationManager = navigationManager;
            this.baseUri = new Uri($"{navigationManager.BaseUri}odata/OSPortDB/");
        }


        public async System.Threading.Tasks.Task ExportArchitecturesToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/osportdb/architectures/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/osportdb/architectures/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async System.Threading.Tasks.Task ExportArchitecturesToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/osportdb/architectures/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/osportdb/architectures/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnGetArchitectures(HttpRequestMessage requestMessage);

        public async Task<Radzen.ODataServiceResult<Osporting.Server.Models.OSPortDB.Architecture>> GetArchitectures(Query query)
        {
            return await GetArchitectures(filter:$"{query.Filter}", orderby:$"{query.OrderBy}", top:query.Top, skip:query.Skip, count:query.Top != null && query.Skip != null);
        }

        public async Task<Radzen.ODataServiceResult<Osporting.Server.Models.OSPortDB.Architecture>> GetArchitectures(string filter = default(string), string orderby = default(string), string expand = default(string), int? top = default(int?), int? skip = default(int?), bool? count = default(bool?), string format = default(string), string select = default(string))
        {
            var uri = new Uri(baseUri, $"Architectures");
            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:filter, top:top, skip:skip, orderby:orderby, expand:expand, select:select, count:count);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetArchitectures(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<Radzen.ODataServiceResult<Osporting.Server.Models.OSPortDB.Architecture>>(response);
        }

        partial void OnCreateArchitecture(HttpRequestMessage requestMessage);

        public async Task<Osporting.Server.Models.OSPortDB.Architecture> CreateArchitecture(Osporting.Server.Models.OSPortDB.Architecture architecture = default(Osporting.Server.Models.OSPortDB.Architecture))
        {
            var uri = new Uri(baseUri, $"Architectures");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, uri);

            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(architecture), Encoding.UTF8, "application/json");

            OnCreateArchitecture(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<Osporting.Server.Models.OSPortDB.Architecture>(response);
        }

        partial void OnDeleteArchitecture(HttpRequestMessage requestMessage);

        public async Task<HttpResponseMessage> DeleteArchitecture(int architectureId = default(int))
        {
            var uri = new Uri(baseUri, $"Architectures({architectureId})");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Delete, uri);

            OnDeleteArchitecture(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }

        partial void OnGetArchitectureByArchitectureId(HttpRequestMessage requestMessage);

        public async Task<Osporting.Server.Models.OSPortDB.Architecture> GetArchitectureByArchitectureId(string expand = default(string), int architectureId = default(int))
        {
            var uri = new Uri(baseUri, $"Architectures({architectureId})");

            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:null, top:null, skip:null, orderby:null, expand:expand, select:null, count:null);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetArchitectureByArchitectureId(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<Osporting.Server.Models.OSPortDB.Architecture>(response);
        }

        partial void OnUpdateArchitecture(HttpRequestMessage requestMessage);
        
        public async Task<HttpResponseMessage> UpdateArchitecture(int architectureId = default(int), Osporting.Server.Models.OSPortDB.Architecture architecture = default(Osporting.Server.Models.OSPortDB.Architecture))
        {
            var uri = new Uri(baseUri, $"Architectures({architectureId})");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Patch, uri);

            httpRequestMessage.Headers.Add("If-Match", architecture.ETag);    

            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(architecture), Encoding.UTF8, "application/json");

            OnUpdateArchitecture(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }

        public async System.Threading.Tasks.Task ExportDerivativesToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/osportdb/derivatives/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/osportdb/derivatives/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async System.Threading.Tasks.Task ExportDerivativesToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/osportdb/derivatives/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/osportdb/derivatives/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnGetDerivatives(HttpRequestMessage requestMessage);

        public async Task<Radzen.ODataServiceResult<Osporting.Server.Models.OSPortDB.Derivative>> GetDerivatives(Query query)
        {
            return await GetDerivatives(filter:$"{query.Filter}", orderby:$"{query.OrderBy}", top:query.Top, skip:query.Skip, count:query.Top != null && query.Skip != null);
        }

        public async Task<Radzen.ODataServiceResult<Osporting.Server.Models.OSPortDB.Derivative>> GetDerivatives(string filter = default(string), string orderby = default(string), string expand = default(string), int? top = default(int?), int? skip = default(int?), bool? count = default(bool?), string format = default(string), string select = default(string))
        {
            var uri = new Uri(baseUri, $"Derivatives");
            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:filter, top:top, skip:skip, orderby:orderby, expand:expand, select:select, count:count);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetDerivatives(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<Radzen.ODataServiceResult<Osporting.Server.Models.OSPortDB.Derivative>>(response);
        }

        partial void OnCreateDerivative(HttpRequestMessage requestMessage);

        public async Task<Osporting.Server.Models.OSPortDB.Derivative> CreateDerivative(Osporting.Server.Models.OSPortDB.Derivative derivative = default(Osporting.Server.Models.OSPortDB.Derivative))
        {
            var uri = new Uri(baseUri, $"Derivatives");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, uri);

            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(derivative), Encoding.UTF8, "application/json");

            OnCreateDerivative(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<Osporting.Server.Models.OSPortDB.Derivative>(response);
        }

        partial void OnDeleteDerivative(HttpRequestMessage requestMessage);

        public async Task<HttpResponseMessage> DeleteDerivative(int derivativeId = default(int))
        {
            var uri = new Uri(baseUri, $"Derivatives({derivativeId})");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Delete, uri);

            OnDeleteDerivative(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }

        partial void OnGetDerivativeByDerivativeId(HttpRequestMessage requestMessage);

        public async Task<Osporting.Server.Models.OSPortDB.Derivative> GetDerivativeByDerivativeId(string expand = default(string), int derivativeId = default(int))
        {
            var uri = new Uri(baseUri, $"Derivatives({derivativeId})");

            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:null, top:null, skip:null, orderby:null, expand:expand, select:null, count:null);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetDerivativeByDerivativeId(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<Osporting.Server.Models.OSPortDB.Derivative>(response);
        }

        partial void OnUpdateDerivative(HttpRequestMessage requestMessage);
        
        public async Task<HttpResponseMessage> UpdateDerivative(int derivativeId = default(int), Osporting.Server.Models.OSPortDB.Derivative derivative = default(Osporting.Server.Models.OSPortDB.Derivative))
        {
            var uri = new Uri(baseUri, $"Derivatives({derivativeId})");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Patch, uri);

            httpRequestMessage.Headers.Add("If-Match", derivative.ETag);    

            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(derivative), Encoding.UTF8, "application/json");

            OnUpdateDerivative(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }

        public async System.Threading.Tasks.Task ExportHardwarePlanningsToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/osportdb/hardwareplannings/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/osportdb/hardwareplannings/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async System.Threading.Tasks.Task ExportHardwarePlanningsToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/osportdb/hardwareplannings/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/osportdb/hardwareplannings/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnGetHardwarePlannings(HttpRequestMessage requestMessage);

        public async Task<Radzen.ODataServiceResult<Osporting.Server.Models.OSPortDB.HardwarePlanning>> GetHardwarePlannings(Query query)
        {
            return await GetHardwarePlannings(filter:$"{query.Filter}", orderby:$"{query.OrderBy}", top:query.Top, skip:query.Skip, count:query.Top != null && query.Skip != null);
        }

        public async Task<Radzen.ODataServiceResult<Osporting.Server.Models.OSPortDB.HardwarePlanning>> GetHardwarePlannings(string filter = default(string), string orderby = default(string), string expand = default(string), int? top = default(int?), int? skip = default(int?), bool? count = default(bool?), string format = default(string), string select = default(string))
        {
            var uri = new Uri(baseUri, $"HardwarePlannings");
            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:filter, top:top, skip:skip, orderby:orderby, expand:expand, select:select, count:count);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetHardwarePlannings(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<Radzen.ODataServiceResult<Osporting.Server.Models.OSPortDB.HardwarePlanning>>(response);
        }

        partial void OnCreateHardwarePlanning(HttpRequestMessage requestMessage);

        public async Task<Osporting.Server.Models.OSPortDB.HardwarePlanning> CreateHardwarePlanning(Osporting.Server.Models.OSPortDB.HardwarePlanning hardwarePlanning = default(Osporting.Server.Models.OSPortDB.HardwarePlanning))
        {
            var uri = new Uri(baseUri, $"HardwarePlannings");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, uri);

            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(hardwarePlanning), Encoding.UTF8, "application/json");

            OnCreateHardwarePlanning(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<Osporting.Server.Models.OSPortDB.HardwarePlanning>(response);
        }

        partial void OnDeleteHardwarePlanning(HttpRequestMessage requestMessage);

        public async Task<HttpResponseMessage> DeleteHardwarePlanning(int hardwarePlanningId = default(int))
        {
            var uri = new Uri(baseUri, $"HardwarePlannings({hardwarePlanningId})");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Delete, uri);

            OnDeleteHardwarePlanning(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }

        partial void OnGetHardwarePlanningByHardwarePlanningId(HttpRequestMessage requestMessage);

        public async Task<Osporting.Server.Models.OSPortDB.HardwarePlanning> GetHardwarePlanningByHardwarePlanningId(string expand = default(string), int hardwarePlanningId = default(int))
        {
            var uri = new Uri(baseUri, $"HardwarePlannings({hardwarePlanningId})");

            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:null, top:null, skip:null, orderby:null, expand:expand, select:null, count:null);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetHardwarePlanningByHardwarePlanningId(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<Osporting.Server.Models.OSPortDB.HardwarePlanning>(response);
        }

        partial void OnUpdateHardwarePlanning(HttpRequestMessage requestMessage);
        
        public async Task<HttpResponseMessage> UpdateHardwarePlanning(int hardwarePlanningId = default(int), Osporting.Server.Models.OSPortDB.HardwarePlanning hardwarePlanning = default(Osporting.Server.Models.OSPortDB.HardwarePlanning))
        {
            var uri = new Uri(baseUri, $"HardwarePlannings({hardwarePlanningId})");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Patch, uri);

            httpRequestMessage.Headers.Add("If-Match", hardwarePlanning.ETag);    

            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(hardwarePlanning), Encoding.UTF8, "application/json");

            OnUpdateHardwarePlanning(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }

        public async System.Threading.Tasks.Task ExportPeopleToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/osportdb/people/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/osportdb/people/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async System.Threading.Tasks.Task ExportPeopleToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/osportdb/people/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/osportdb/people/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnGetPeople(HttpRequestMessage requestMessage);

        public async Task<Radzen.ODataServiceResult<Osporting.Server.Models.OSPortDB.Person>> GetPeople(Query query)
        {
            return await GetPeople(filter:$"{query.Filter}", orderby:$"{query.OrderBy}", top:query.Top, skip:query.Skip, count:query.Top != null && query.Skip != null);
        }

        public async Task<Radzen.ODataServiceResult<Osporting.Server.Models.OSPortDB.Person>> GetPeople(string filter = default(string), string orderby = default(string), string expand = default(string), int? top = default(int?), int? skip = default(int?), bool? count = default(bool?), string format = default(string), string select = default(string))
        {
            var uri = new Uri(baseUri, $"People");
            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:filter, top:top, skip:skip, orderby:orderby, expand:expand, select:select, count:count);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetPeople(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<Radzen.ODataServiceResult<Osporting.Server.Models.OSPortDB.Person>>(response);
        }

        partial void OnCreatePerson(HttpRequestMessage requestMessage);

        public async Task<Osporting.Server.Models.OSPortDB.Person> CreatePerson(Osporting.Server.Models.OSPortDB.Person person = default(Osporting.Server.Models.OSPortDB.Person))
        {
            var uri = new Uri(baseUri, $"People");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, uri);

            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(person), Encoding.UTF8, "application/json");

            OnCreatePerson(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<Osporting.Server.Models.OSPortDB.Person>(response);
        }

        partial void OnDeletePerson(HttpRequestMessage requestMessage);

        public async Task<HttpResponseMessage> DeletePerson(int personId = default(int))
        {
            var uri = new Uri(baseUri, $"People({personId})");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Delete, uri);

            OnDeletePerson(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }

        partial void OnGetPersonByPersonId(HttpRequestMessage requestMessage);

        public async Task<Osporting.Server.Models.OSPortDB.Person> GetPersonByPersonId(string expand = default(string), int personId = default(int))
        {
            var uri = new Uri(baseUri, $"People({personId})");

            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:null, top:null, skip:null, orderby:null, expand:expand, select:null, count:null);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetPersonByPersonId(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<Osporting.Server.Models.OSPortDB.Person>(response);
        }

        partial void OnUpdatePerson(HttpRequestMessage requestMessage);
        
        public async Task<HttpResponseMessage> UpdatePerson(int personId = default(int), Osporting.Server.Models.OSPortDB.Person person = default(Osporting.Server.Models.OSPortDB.Person))
        {
            var uri = new Uri(baseUri, $"People({personId})");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Patch, uri);

            httpRequestMessage.Headers.Add("If-Match", person.ETag);    

            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(person), Encoding.UTF8, "application/json");

            OnUpdatePerson(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }

        public async System.Threading.Tasks.Task ExportStatusesToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/osportdb/statuses/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/osportdb/statuses/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async System.Threading.Tasks.Task ExportStatusesToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/osportdb/statuses/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/osportdb/statuses/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnGetStatuses(HttpRequestMessage requestMessage);

        public async Task<Radzen.ODataServiceResult<Osporting.Server.Models.OSPortDB.Status>> GetStatuses(Query query)
        {
            return await GetStatuses(filter:$"{query.Filter}", orderby:$"{query.OrderBy}", top:query.Top, skip:query.Skip, count:query.Top != null && query.Skip != null);
        }

        public async Task<Radzen.ODataServiceResult<Osporting.Server.Models.OSPortDB.Status>> GetStatuses(string filter = default(string), string orderby = default(string), string expand = default(string), int? top = default(int?), int? skip = default(int?), bool? count = default(bool?), string format = default(string), string select = default(string))
        {
            var uri = new Uri(baseUri, $"Statuses");
            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:filter, top:top, skip:skip, orderby:orderby, expand:expand, select:select, count:count);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetStatuses(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<Radzen.ODataServiceResult<Osporting.Server.Models.OSPortDB.Status>>(response);
        }

        partial void OnCreateStatus(HttpRequestMessage requestMessage);

        public async Task<Osporting.Server.Models.OSPortDB.Status> CreateStatus(Osporting.Server.Models.OSPortDB.Status status = default(Osporting.Server.Models.OSPortDB.Status))
        {
            var uri = new Uri(baseUri, $"Statuses");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, uri);

            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(status), Encoding.UTF8, "application/json");

            OnCreateStatus(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<Osporting.Server.Models.OSPortDB.Status>(response);
        }

        partial void OnDeleteStatus(HttpRequestMessage requestMessage);

        public async Task<HttpResponseMessage> DeleteStatus(int statusId = default(int))
        {
            var uri = new Uri(baseUri, $"Statuses({statusId})");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Delete, uri);

            OnDeleteStatus(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }

        partial void OnGetStatusByStatusId(HttpRequestMessage requestMessage);

        public async Task<Osporting.Server.Models.OSPortDB.Status> GetStatusByStatusId(string expand = default(string), int statusId = default(int))
        {
            var uri = new Uri(baseUri, $"Statuses({statusId})");

            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:null, top:null, skip:null, orderby:null, expand:expand, select:null, count:null);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetStatusByStatusId(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<Osporting.Server.Models.OSPortDB.Status>(response);
        }

        partial void OnUpdateStatus(HttpRequestMessage requestMessage);
        
        public async Task<HttpResponseMessage> UpdateStatus(int statusId = default(int), Osporting.Server.Models.OSPortDB.Status status = default(Osporting.Server.Models.OSPortDB.Status))
        {
            var uri = new Uri(baseUri, $"Statuses({statusId})");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Patch, uri);

            httpRequestMessage.Headers.Add("If-Match", status.ETag);    

            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(status), Encoding.UTF8, "application/json");

            OnUpdateStatus(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }

        public async System.Threading.Tasks.Task ExportTestPcsToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/osportdb/testpcs/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/osportdb/testpcs/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async System.Threading.Tasks.Task ExportTestPcsToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/osportdb/testpcs/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/osportdb/testpcs/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnGetTestPcs(HttpRequestMessage requestMessage);

        public async Task<Radzen.ODataServiceResult<Osporting.Server.Models.OSPortDB.TestPc>> GetTestPcs(Query query)
        {
            return await GetTestPcs(filter:$"{query.Filter}", orderby:$"{query.OrderBy}", top:query.Top, skip:query.Skip, count:query.Top != null && query.Skip != null);
        }

        public async Task<Radzen.ODataServiceResult<Osporting.Server.Models.OSPortDB.TestPc>> GetTestPcs(string filter = default(string), string orderby = default(string), string expand = default(string), int? top = default(int?), int? skip = default(int?), bool? count = default(bool?), string format = default(string), string select = default(string))
        {
            var uri = new Uri(baseUri, $"TestPcs");
            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:filter, top:top, skip:skip, orderby:orderby, expand:expand, select:select, count:count);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetTestPcs(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<Radzen.ODataServiceResult<Osporting.Server.Models.OSPortDB.TestPc>>(response);
        }

        partial void OnCreateTestPc(HttpRequestMessage requestMessage);

        public async Task<Osporting.Server.Models.OSPortDB.TestPc> CreateTestPc(Osporting.Server.Models.OSPortDB.TestPc testPc = default(Osporting.Server.Models.OSPortDB.TestPc))
        {
            var uri = new Uri(baseUri, $"TestPcs");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, uri);

            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(testPc), Encoding.UTF8, "application/json");

            OnCreateTestPc(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<Osporting.Server.Models.OSPortDB.TestPc>(response);
        }

        partial void OnDeleteTestPc(HttpRequestMessage requestMessage);

        public async Task<HttpResponseMessage> DeleteTestPc(int testPcid = default(int))
        {
            var uri = new Uri(baseUri, $"TestPcs({testPcid})");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Delete, uri);

            OnDeleteTestPc(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }

        partial void OnGetTestPcByTestPcid(HttpRequestMessage requestMessage);

        public async Task<Osporting.Server.Models.OSPortDB.TestPc> GetTestPcByTestPcid(string expand = default(string), int testPcid = default(int))
        {
            var uri = new Uri(baseUri, $"TestPcs({testPcid})");

            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:null, top:null, skip:null, orderby:null, expand:expand, select:null, count:null);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetTestPcByTestPcid(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<Osporting.Server.Models.OSPortDB.TestPc>(response);
        }

        partial void OnUpdateTestPc(HttpRequestMessage requestMessage);
        
        public async Task<HttpResponseMessage> UpdateTestPc(int testPcid = default(int), Osporting.Server.Models.OSPortDB.TestPc testPc = default(Osporting.Server.Models.OSPortDB.TestPc))
        {
            var uri = new Uri(baseUri, $"TestPcs({testPcid})");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Patch, uri);

            httpRequestMessage.Headers.Add("If-Match", testPc.ETag);    

            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(testPc), Encoding.UTF8, "application/json");

            OnUpdateTestPc(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }
    }
}