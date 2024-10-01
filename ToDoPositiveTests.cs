using FluentAssertions;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace test2
{
    public class IntegrationTests
    {
        private readonly HttpClient _client;

        public IntegrationTests()
        {
            // Inicializa el cliente HTTP con la URL base de tu API real
            _client = new HttpClient
            {
                BaseAddress = new Uri("https://todo-blackbird-3.onrender.com/api/")
            };
        }

        [Fact]
        public async Task GetAllToDos_ReturnsSuccessStatusCode()
        {
            // Envía una solicitud GET a la API para obtener todos los ToDos
            var response = await _client.GetAsync("ToDo/GetAllToDos");

            // Verifica que la respuesta sea exitosa (código de estado 200)
            response.StatusCode.Should().Be(HttpStatusCode.OK, "la API debería retornar un código de estado 200 OK");

            // Verifica que el contenido de la respuesta no sea nulo ni vacío
            var responseData = await response.Content.ReadAsStringAsync();
            responseData.Should().NotBeNullOrEmpty("la respuesta no debería ser nula o vacía");
        }


        [Fact]
        public async Task GetAllToDosDelete_ReturnsSuccessStatusCode()
        {
            // Envía una solicitud GET a la API para obtener todos los ToDos eliminados
            var response = await _client.GetAsync("ToDo/GetAllToDosDelete");

            // Verifica que la respuesta sea exitosa (código de estado 200)
            response.StatusCode.Should().Be(HttpStatusCode.OK, "la API debería retornar un código de estado 200 OK");

            // Verifica que el contenido de la respuesta no sea nulo ni vacío
            var responseData = await response.Content.ReadAsStringAsync();
            responseData.Should().NotBeNullOrEmpty("la respuesta no debería ser nula o vacía");
        }


        [Fact]
        public async Task GetToDoById_ReturnsSuccessStatusCode_WhenExists()
        {
            // Realiza una solicitud GET para obtener un ToDo específico por su ID
            var response =
                await _client.GetAsync("ToDo/GetToDoById/66edc6866001ea84da10b60b"); // Asegúrate de que este ID exista

            // Verifica que la respuesta sea exitosa (código de estado 200)
            response.StatusCode.Should().Be(HttpStatusCode.OK, "la API debería retornar un código de estado 200 OK");

            // Verifica que el contenido de la respuesta no sea nulo ni vacío
            var responseData = await response.Content.ReadAsStringAsync();
            responseData.Should().NotBeNullOrEmpty("la respuesta no debería ser nula o vacía");
        }

        [Fact]
        public async Task CreateToDo_ReturnsSuccessStatusCode_WhenCreated()
        {
            // Crea un nuevo ToDo para enviar a la API
            var newToDo = new ToDoItem
            {
                Name = "Test Task",
                Description = "Test Description"
            };

            // Serializa el ToDo en formato JSON
            var jsonContent = Newtonsoft.Json.JsonConvert.SerializeObject(newToDo);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            // Realiza una solicitud POST para crear un nuevo ToDo
            var response = await _client.PostAsync("ToDoCreate/CreateToDo", content);

            // Verifica que la respuesta tenga un código de estado 201 Created
            response.StatusCode.Should().Be(HttpStatusCode.Created,
                "la API debería retornar un código de estado 201 Created");

            // Verifica que el contenido de la respuesta no sea nulo
            var responseData = await response.Content.ReadAsStringAsync();
            responseData.Should().NotBeNullOrEmpty("la respuesta no debería ser nula o vacía");
        }
        
        [Fact]
        public async Task UpdateToDo_ReturnsSuccessStatusCode_WhenUpdated()
        {
            // Crea un ToDo existente para enviar a la API
            var updatedToDo = new ToDoItem
            {
                id = "66edc6856001ea84da10b60a",
                Name = "Updated Test Task",
                Description = "Updated Test Description"
            };

            // Serializa el ToDo en formato JSON
            var jsonContent = Newtonsoft.Json.JsonConvert.SerializeObject(updatedToDo);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            // Realiza una solicitud PUT para actualizar el ToDo
            var response = await _client.PutAsync($"ToDoUpdate/UpdateToDo/{updatedToDo.id}", content);

            // Verifica que la respuesta tenga un código de estado 200 OK
            response.StatusCode.Should().Be(HttpStatusCode.OK, "la API debería retornar un código de estado 200 OK");

            // Verifica que el contenido de la respuesta no sea nulo
            var responseData = await response.Content.ReadAsStringAsync();
            responseData.Should().NotBeNullOrEmpty("la respuesta no debería ser nula o vacía");
        }
        
        
        [Fact]
        public async Task UpdateStatusToDo_ReturnsSuccessStatusCode_WhenUpdated()
        {
            // Solo se envía el estado en el JSON
            var updateStatusToDo = new
            {
                Status = "2" // Actualiza el estado, en este caso "2"
            };

            // Serializa el objeto que contiene solo el estado en formato JSON
            var jsonContent = Newtonsoft.Json.JsonConvert.SerializeObject(updateStatusToDo);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            // ID del ToDo que se va a actualizar
            var todoId = "66fb3490215a0ba7a87f0fba"; // El ID se especifica en el endpoint

            // Realiza una solicitud PUT para actualizar el estado del ToDo
            var response = await _client.PutAsync($"ToDoUpdate/UpdateStatusToDo/{todoId}", content);

            // Verifica que la respuesta tenga un código de estado 200 OK
            response.StatusCode.Should().Be(HttpStatusCode.OK, "la API debería retornar un código de estado 200 OK");

            // Verifica que el contenido de la respuesta no sea nulo
            var responseData = await response.Content.ReadAsStringAsync();
            responseData.Should().NotBeNullOrEmpty("la respuesta no debería ser nula o vacía");
        }




        [Fact]
        public async Task DeleteToDo_ReturnsSuccessStatusCode_WhenDeleted()
        {
            // Realiza una solicitud DELETE para eliminar un ToDo existente
            var response =
                await _client.DeleteAsync(
                    "ToDoUpdate/SoftDeleteToDo/66fb3aab215a0ba7a87f0fbe"); // Asegúrate de que este ID exista

            // Verifica que la respuesta tenga un código de estado 204 No Content
            response.StatusCode.Should().Be(HttpStatusCode.NoContent,
                "la API debería retornar un código de estado 204 No Content");
        }
    }

    // Modelo para deserializar la respuesta de la API
    public class ToDoItem
    {
        public string id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Date_Create { get; set; }
        public string Date_Finish { get; set; }
        public string Status { get; set; }
        public bool IsActive { get; set; }
    }
}