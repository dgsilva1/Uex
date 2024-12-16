namespace TesteUex.Models.CordinatesResponse
{
    public class CordinatesResponse
    {
        public List<Result>? Results { get; set; }
        public string? Status { get; set; }
    }

    public class Result
    {
        public List<AddressComponent>? AddressComponents { get; set; }
        public string? FormattedAddress { get; set; }
        public Geometry? Geometry { get; set; }
        public List<NavigationPoint>? NavigationPoints { get; set; }
        public string? PlaceId { get; set; }
        public List<string>? Types { get; set; }
    }

    public class AddressComponent
    {
        public string? LongName { get; set; }
        public string? ShortName { get; set; }
        public List<string>? Types { get; set; }
    }

    public class Geometry
    {
        public Location? Location { get; set; }
        public Bounds? Bounds { get; set; }
    }

    public class Location
    {
        public double? Lat { get; set; }
        public double? Lng { get; set; }
    }

    public class Bounds
    {
        public LatLng? Northeast { get; set; }
        public LatLng? Southwest { get; set; }
    }

    public class LatLng
    {
        public double? Lat { get; set; }
        public double? Lng { get; set; }
    }

    public class NavigationPoint
    {
        public Location? Location { get; set; }
    }
}