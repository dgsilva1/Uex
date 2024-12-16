using API.Extensions;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TesteUex.Entities;
using TesteUex.Extensions;
using TesteUex.Helpers;
using TesteUex.Interfaces;
using TesteUex.Models.AccountModels;
using TesteUex.Models.ContactModels;
using TesteUex.Models.CordinatesResponse;

namespace TesteUex.Controllers
{
    [Authorize]
    public class ContactController(IUserRepository userRepository, IContactRepository contactRepository, IMapper mapper) : BaseApiController
    {
        private readonly string API_KEY = "AIzaSyAOBwLBSpqLOOQuGnzJLx4VFDXMh9N-qHo";

        [HttpGet("contacts")]
        public async Task<ActionResult<ContactModel>> GetContacts()
        {
            var user = await userRepository.GetUserByLoginAsync(User.GetLoginUsuario());

            if (user == null)
                return BadRequest("Não foi possível obter o login pela sessão, tente relogar.");

            var contacts = user.Contacts?.AsQueryable();

            if (contacts == null || !contacts.Any())
                return NoContent();

            var mappedContacts = mapper.Map<List<ContactModel>>(contacts.ToList());

            foreach (var contact in mappedContacts)
            {
                contact.Cpf = contact.Cpf.ToFormat("###.###.###-##");
                contact.Phone = Validators.FormatarTelefone(contact.Phone);
            }

            return Ok(mappedContacts);
        }

        [HttpGet("search/{cep}")]
        public async Task<IActionResult> SearchCep(string cep)
        {
            try
            {
                if (string.IsNullOrEmpty(cep) || cep.Length != 8)
                {
                    return BadRequest("CEP inválido ou vazio");
                }

                using (var httpClient = new HttpClient())
                {
                    var response = await httpClient.GetAsync($"https://viacep.com.br/ws/{cep}/json/");
                    if (response.IsSuccessStatusCode)
                    {
                        var jsonResponse = await response.Content.ReadAsStringAsync();
                        return Ok(jsonResponse);
                    }

                    return StatusCode((int)response.StatusCode, "Erro ao acessar Via Cep");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno no servidor: {ex.Message}");
            }
        }

        [NonAction]
        public async Task<IActionResult> SearchCoordinates(string cep, string street, int number)
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    var response = await httpClient.GetAsync($"https://maps.googleapis.com/maps/api/geocode/json?address={cep}, {street}, {number}&key={API_KEY}");

                    if (response.IsSuccessStatusCode)
                    {
                        var jsonResponse = await response.Content.ReadAsStringAsync();
                        var mapsResponse = JsonConvert.DeserializeObject<CordinatesResponse>(jsonResponse);

                        if (mapsResponse?.Results?.Count > 0)
                        {
                            var firstResult = mapsResponse.Results[0];
                            var coordinates = firstResult?.Geometry?.Location;

                            if (coordinates != null)
                            {
                                return Ok(new
                                {
                                    Latitude = coordinates.Lat,
                                    Longitude = coordinates.Lng,
                                    Address = firstResult?.FormattedAddress
                                });
                            }
                        }
                        return StatusCode(404, "Nenhuma localização encontrada.");
                    }

                    return StatusCode((int)response.StatusCode, "Erro ao acessar o Google Maps.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno no servidor: {ex.Message}");
            }
        }

        [HttpPut("update-contact")]
        public async Task<ActionResult> UpdateContact(ContactUpdateModel contactUpdateModel)
        {
            var user = await userRepository.GetUserByLoginAsync(User.GetLoginUsuario());

            if (user == null)
                return BadRequest("Não foi possível obter o login pela sessão, tente relogar.");

            var contact = await contactRepository.GetContactById(contactUpdateModel.ContactId);

            if (contact == null)
                return BadRequest("Não foi possível localizar o contato.");

            if (contact.UserId != user.UserId)
                return Unauthorized("Você não tem permissão para alterar esse contato.");

            if (!string.IsNullOrEmpty(contactUpdateModel.Cpf) && !Validators.ValidateCpf(contactUpdateModel.Cpf))
                return BadRequest("Cpf inválido");

            if (!string.IsNullOrEmpty(contactUpdateModel.Phone) && (!Validators.ValidateDdd(contactUpdateModel.Phone.GetNumbers().Substring(0, 2))
                || !Validators.ValidatePhone(contactUpdateModel.Phone.GetNumbers().Substring(2))))
                return BadRequest("Celular inválido");

            if (contactUpdateModel.Cpf.GetNumbers() != contact.Cpf.GetNumbers() &&
                    await contactRepository.CheckCpfExists(user.UserId, contactUpdateModel.Cpf))
                return BadRequest("Cpf já vinculado à outro contato");

            contactUpdateModel.Cpf = contactUpdateModel.Cpf.GetNumbers();
            contactUpdateModel.Phone = contactUpdateModel.Phone.GetNumbers();
            contactUpdateModel.Cep = contactUpdateModel.Cep.GetNumbers();

            var coordenadas = await SearchCoordinates(contactUpdateModel.Cep, contactUpdateModel.Street, contactUpdateModel.Number);

            if (coordenadas is OkObjectResult okResult)
            {
                var mapsResponse = okResult.Value as CordinatesResponse;

                if (mapsResponse?.Results?.Count > 0)
                {
                    var firstResult = mapsResponse.Results[0];
                    var location = firstResult?.Geometry?.Location;

                    if (location != null)
                    {
                        contactUpdateModel.Latitude = location.Lat ?? 0;
                        contactUpdateModel.Longitude = location.Lng ?? 0;
                    }
                }
            }

            mapper.Map(contactUpdateModel, contact);

            if (await contactRepository.SaveAllAsync())
                return NoContent();

            return BadRequest("Falha ao atualizar o contato.");
        }

        [HttpPost("register")]
        public async Task<ActionResult<User>> Register(RegisterContactModel model)
        {
            var user = await userRepository.GetUserByLoginAsync(User.GetLoginUsuario());

            if (user == null)
                return BadRequest("Não foi possível obter o login pela sessão, tente relogar.");

            if (!Validators.ValidateCpf(model.Cpf))
                return BadRequest("Cpf inválido");

            if (!Validators.ValidateDdd(model.Phone.Substring(0, 2)) || !Validators.ValidatePhone(model.Phone.GetNumbers().Substring(2)))
                return BadRequest("Celular inválido");

            if (await contactRepository.CheckCpfExists(user.UserId, model.Cpf))
                return BadRequest("Cpf já vinculado à outro contato");

            var coordenadas = await SearchCoordinates(model.Cep, model.Street, model.Number);

            if (coordenadas is OkObjectResult okResult)
            {
                var mapsResponse = okResult.Value as CordinatesResponse;

                if (mapsResponse?.Results?.Count > 0)
                {
                    var firstResult = mapsResponse.Results[0];
                    var location = firstResult?.Geometry?.Location;

                    if (location != null)
                    {
                        model.Latitude = location.Lat ?? 0;
                        model.Longitude = location.Lng ?? 0;
                    }
                }
            }

            var contact = new Contact
            {
                Name = model.Name,
                Cpf = model.Cpf.GetNumbers(),
                Phone = model.Phone.GetNumbers(),
                UserId = user.UserId,
                Street = model.Street,
                State = model.State,
                City = model.City,
                District = model.District,
                Cep = model.Cep,
                Number = model.Number,
                Complement = model.Complement,
                Latitude = model.Latitude,
                Longitude = model.Longitude

            };


            user.Contacts?.Add(contact);

            if (await userRepository.SaveAllAsync())
                return CreatedAtAction(nameof(GetUser), new { userId = user.UserId }, mapper.Map<ContactModel>(contact));

            return BadRequest("Ocorreu um erro ao adicionar o contato.");
        }

        [HttpDelete("delete-contact/{contactId:int}")]
        public async Task<ActionResult> DeleteContact(int contactId)
        {
            var user = await userRepository.GetUserByLoginAsync(User.GetLoginUsuario());

            if (user == null)
                return BadRequest("Não foi possível obter o login pela sessão, tente relogar.");

            var contact = await contactRepository.GetContactById(contactId);

            if (contact == null)
                return NotFound("Não foi possível localizar o contato.");

            if (contact.UserId != user.UserId)
                return Unauthorized("Você não tem permissão para excluir esse contato.");


            user.Contacts?.Remove(contact);

            if (await userRepository.SaveAllAsync())
                return Ok();

            return BadRequest("Erro ao excluir contato.");
        }

        [HttpGet("{userId}")] //api/users/{login}
        public async Task<ActionResult<UserModel>> GetUser(int userId)
        {
            var usuario = await userRepository.GetUserModelByLoginAsync(userId);

            if (usuario == null)
                return NotFound();

            return usuario;
        }
    }
}
