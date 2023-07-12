namespace MyProject.ViewModels.Footer;

public class FooterRequest: PagingVm
{
    /// <summary>
    /// 1. Footer main
    /// 2. Footer blog
    /// </summary>
    public int? Status { get; set; }
}
