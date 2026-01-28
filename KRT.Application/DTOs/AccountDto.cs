using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KRT.Application.DTOs
{
    public record AccountDto
    (
        string Document, 
        string AccountNumber, 
        string AgencyNumber,
        decimal PixLimit 
    );
}
