using System;

namespace backend.Models
{
    /// <summary>
    /// Modelo para mostrar errores
    /// </summary>
    public class ErrorViewModel
    {
        public string RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}
