using System.Text.Json.Nodes;

namespace Play.Catalog.Contract
{
    public class Contracts
    {
        public record CatalogItemCreated(Guid ItemId, string Name, string Description);
        public record CatalogItemUpdated(Guid ItemId, string Name, string Description);
        public record CatalogItemDelteted(Guid ItemId);

        public record LogActions(string NombreOpcion, string TipoDocumento, string NumeroDocumento, string Solicitud, string Respuesta, string? Ip, DateTime Fecha);

    }
}