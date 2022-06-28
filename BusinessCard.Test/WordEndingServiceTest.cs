using BusinessCard.BusinessLogicLayer.Services;
using Xunit;

namespace BusinessCard.Test
{
    public class WordEndingServiceTest
    {
        [Fact]
        public void GetWordForZeroProjects()
        {
            var result = new ProjectWordEndingService().GetWord(0);

            Assert.Equal("Проектов", result);
        }

        [Fact]
        public void GetWordForOneProject()
        {
            var result = new ProjectWordEndingService().GetWord(12);

            Assert.Equal("Проектов", result);
        }

        [Fact]
        public void GetWordForZeroDownloads()
        {
            var result = new DownloadWordEndingService().GetWord(0);

            Assert.Equal("Скачиваний", result);
        }

        [Fact]
        public void GetWordForOneDownload()
        {
            var result = new DownloadWordEndingService().GetWord(22);

            Assert.Equal("Скачивания", result);
        }
    }
}