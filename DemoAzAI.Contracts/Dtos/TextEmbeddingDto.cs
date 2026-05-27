using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoAzAI.Contracts.Dtos
{
    public record TextEmbeddingDto
    (
        Guid Guid,
        string Text,
        float[] Vector
    );
}
