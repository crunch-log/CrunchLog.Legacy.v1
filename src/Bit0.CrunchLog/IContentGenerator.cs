namespace Bit0.CrunchLog
{
    public interface IContentGenerator
    {
        void CleanOutput();
        void Publish();
        void PublishArchive();
        void PublishCategories();
        void PublishContent();
        void PublishHome();
        void PublishTags();
        void PublishImages();
    }
}