using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Qurigo.Interfaces;
using Qurigo.State.VectorState;
using Swashbuckle.AspNetCore.Annotations;
using System.Numerics;

namespace Qurigo.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ExecuteController : ControllerBase
{
    private readonly ICircuit _circuit;

    public ExecuteController(ICircuit circuit)
    {
        _circuit = circuit;
    }

    [HttpPost]
    public ActionResult<IEnumerable<Complex>> ExecuteProgram(string program)
    {
        _circuit.ExecuteProgram(program);
        return Ok(_circuit.GetState().ToArray().AsEnumerable());
    }
}
