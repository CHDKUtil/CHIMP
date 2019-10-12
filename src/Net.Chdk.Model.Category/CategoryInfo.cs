namespace Net.Chdk.Model.Category
{
    public sealed class CategoryInfo
    {
        public string? Name { get; set; }

        public override bool Equals(object obj)
        {
            return obj is CategoryInfo categoryInfo2
                && Name?.Equals(categoryInfo2.Name) == true;
        }

        public override int GetHashCode()
        {
            return Name != null
                ? Name.GetHashCode()
                : 0;
        }

        public static bool operator ==(CategoryInfo categoryInfo1, CategoryInfo categoryInfo2)
        {
            if (ReferenceEquals(categoryInfo1, categoryInfo2))
                return true;
            if (categoryInfo1 is null || categoryInfo2 is null)
                return false;
            return categoryInfo1.Equals(categoryInfo2);
        }

        public static bool operator !=(CategoryInfo categoryInfo1, CategoryInfo categoryInfo2)
        {
            return !(categoryInfo1 == categoryInfo2);
        }
    }
}
