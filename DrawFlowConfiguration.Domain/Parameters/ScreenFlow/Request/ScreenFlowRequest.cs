using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrawFlowConfiguration.Domain.Parameters.ScreenFlow.Request
{
    public class ScreenFlowRequest
    {
       // public int ScreenFlowID { get; set; }               // Id del flujo
        public Guid AgreementID { get; set; }               // GUID del convenio
        public int? SelectedIdWorkflow { get; set; }
        public int? ContScreenFlow { get; set; }
        public int? OperationScreenFlowID { get; set; }       // Operación o ID relacionado
        public bool StateScreenFlow { get; set; } = true;   // Estado (activo/inactivo)
        public int? CreatorUserID { get; set; }             // ID del usuario creador
        public DateTime CreatedDate { get; set; }           // Fecha de creación
        public DateTime? UpdatedDate { get; set; }          // Fecha de modificación
        public bool Active { get; set; } = true;            // Registro activo o no
        public bool IsDeleted { get; set; } = false;        // Borrado lógico

    }
}
