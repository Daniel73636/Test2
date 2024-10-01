using FluentAssertions;
using System.Net;
using System.Text;

namespace test2
{
    public class IntegrationTestsNegative
    {
        private readonly HttpClient _httpClientTest;

        public IntegrationTestsNegative()
        {
            // Inicializa el cliente HTTP con la URL base de tu API real
            _httpClientTest = new HttpClient
            {
                BaseAddress = new Uri("https://todo-blackbird-3.onrender.com/api/")
            };
        }

        // Prueba negativa para validar que devuelve 404 cuando el ToDo no existe
        [Fact]
        public async Task GetToDoById_ReturnsNotFound_WhenToDoDoesNotExist()
        {
            // Solicita un ID que no existe
            var httpResponseNotFound = await _httpClientTest.GetAsync("ToDo/GetToDoById/nonexistent-id");

            // Verifica que la respuesta sea 404 Not Found
            httpResponseNotFound.StatusCode.Should().Be(HttpStatusCode.NotFound, "la API debería retornar 404 si el ToDo no existe");
        }

        // Prueba negativa para validar que devuelve 400 cuando se envían datos inválidos en la creación
        [Fact]
        public async Task CreateToDo_ReturnsBadRequest_WhenInvalidData()
        {
            // Crea un ToDo con datos inválidos (e.g., sin nombre)
            var toDoInvalido = new ToDoItem2
            {
                Description = "Descripción sin nombre"
            };

            var jsonInvalido = Newtonsoft.Json.JsonConvert.SerializeObject(toDoInvalido);
            var contenidoInvalido = new StringContent(jsonInvalido, Encoding.UTF8, "application/json");

            // Realiza una solicitud POST con datos inválidos
            var httpResponseBadRequest = await _httpClientTest.PostAsync("ToDoCreate/CreateToDo", contenidoInvalido);

            // Verifica que la respuesta tenga un código de estado 400 Bad Request
            httpResponseBadRequest.StatusCode.Should().Be(HttpStatusCode.BadRequest, "la API debería retornar un código de estado 400 Bad Request cuando los datos son inválidos");
        }

        // Prueba negativa para validar que devuelve 404 cuando intenta actualizar un ToDo que no existe
        [Fact]
        public async Task UpdateToDo_ReturnsNotFound_WhenToDoDoesNotExist()
        {
            // Intenta actualizar un ToDo inexistente
            var toDoNoExistente = new ToDoItem2
            {
                id = "nonexistent-id",
                Name = "Tarea Actualizada",
                Description = "Descripción Actualizada"
            };

            var jsonToDoNoExistente = Newtonsoft.Json.JsonConvert.SerializeObject(toDoNoExistente);
            var contenidoActualizacion = new StringContent(jsonToDoNoExistente, Encoding.UTF8, "application/json");

            var httpResponseNoExistenteUpdate = await _httpClientTest.PutAsync($"ToDoUpdate/UpdateToDo/{toDoNoExistente.id}", contenidoActualizacion);

            // Verifica que la respuesta sea 404 Not Found
            httpResponseNoExistenteUpdate.StatusCode.Should().Be(HttpStatusCode.NotFound, "la API debería retornar 404 si el ToDo no existe");
        }

        // Prueba negativa para validar que devuelve 404 cuando intenta eliminar un ToDo que no existe
        [Fact]
        public async Task DeleteToDo_ReturnsNotFound_WhenToDoDoesNotExist()
        {
            // Intenta eliminar un ToDo inexistente
            var httpResponseNoExistenteDelete = await _httpClientTest.DeleteAsync("ToDoUpdate/SoftDeleteToDo/nonexistent-id");

            // Verifica que la respuesta sea 404 Not Found
            httpResponseNoExistenteDelete.StatusCode.Should().Be(HttpStatusCode.NotFound, "la API debería retornar 404 si el ToDo no existe");
        }

        // Prueba negativa para validar que devuelve 500 si hay un error en el servidor
        [Fact]
        public async Task GetAllToDos_ReturnsInternalServerError_WhenServerErrorOccurs()
        {
            // Modifica el endpoint para provocar un error del servidor
            var httpResponseServerError = await _httpClientTest.GetAsync("ToDo/TriggerServerError");

            // Verifica que la respuesta sea 500 Internal Server Error
            httpResponseServerError.StatusCode.Should().Be(HttpStatusCode.InternalServerError, "la API debería retornar 500 si ocurre un error interno en el servidor");
        }
    }

    // Modelo para deserializar la respuesta de la API
    public class ToDoItem2
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
