namespace MyProject.ViewModels.Category;

public class CategoryTreePagingVm: PagingVm
{
    public int? Level { get; set; }
}

public class CategoryHotVm: PagingVm
{
    // 3. Hot category
    // 4. Good Accessory 
    public int Status { get; set; }
}

public class CategoryChildrenPagingVm : PagingVm
{
}
public class CategoryRequest : PagingVm
{
}

