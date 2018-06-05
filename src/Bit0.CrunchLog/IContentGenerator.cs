namespace Bit0.CrunchLog
{
    public interface IContentGenerator
    {
        void CleanOutput();
        void Publish();
        void PublishArchive();
        void PublishCategories();
        void PublishContent();
        void PublisHome();
        void PublishTags();
        void PublishImages();
    }
}