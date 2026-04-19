namespace One.MDM.Authentication.Model
{
    public class PageInfo
    {
        public int Number { get; set; }
        public int Size { get; set; }
        public int TotalElements { get; set; }
        public int TotalPages { get; set; }
        public bool First { get; set; }
        public bool Last { get; set; }
    }

    public class PagedResult<T>
    {
        public List<T> Data { get; set; }
        public PageInfo Page { get; set; }
    }

}
