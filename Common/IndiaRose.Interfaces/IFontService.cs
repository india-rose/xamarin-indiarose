using System.Collections.Generic;

namespace IndiaRose.Interfaces
{
    /// <summary>
    /// Contient la liste des polices du device
    /// </summary>
    public interface IFontService
    {
        /// <summary>
        /// Dictionnaire contenant les polices
        /// </summary>
        Dictionary<string, string> FontList { get; }
    }
}
