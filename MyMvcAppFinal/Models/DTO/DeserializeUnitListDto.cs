namespace MyMvcAppFinal.Models.DTO
{
    public class DeserializeUnitDto
    {
        [Newtonsoft.Json.JsonProperty("name")]
        public string Name { get; set; }
        [Newtonsoft.Json.JsonProperty("parent")]
        public string? ParentName { get; set; }
    }

    public class DeserializeUnitListDto
    {
        [Newtonsoft.Json.JsonProperty("units")]
        public List<DeserializeUnitDto> Units { get; set; }
    }
}
