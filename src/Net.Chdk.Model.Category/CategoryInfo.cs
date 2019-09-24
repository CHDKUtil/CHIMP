namespace Net.Chdk.Model.Category
{
    public sealed class CategoryInfo
    {
        public CategoryInfo(string name)
        {
            Name = name;
        }

        public string Name { get; }

        public override bool Equals(object obj)
        {
            var categoryInfo2 = obj as CategoryInfo;
            return Name.Equals(categoryInfo2?.Name);
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode();
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
