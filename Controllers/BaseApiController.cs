using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExpensesTracker.Controllers;

[ApiController, Authorize, Route("api/[controller]")]
public class BaseApiController : ControllerBase { }
