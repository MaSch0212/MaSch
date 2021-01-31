namespace MaSch.Presentation.Wpf.Views.SplitView
{
    public interface IPageGroup
    {
        string PageGroupId { get; }
        string PageGroupName { get; }
        string TranslationProviderKey { get; }
        int PageGroupPriority { get; }
    }
}
