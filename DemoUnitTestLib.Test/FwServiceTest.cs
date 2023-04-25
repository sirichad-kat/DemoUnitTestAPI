using DemoUnitTestLib.BL;
using DemoUnitTestLib.DL;
using Moq; 

namespace DemoUnitTestLib.Test
{
    public class FwServiceTest
    {
        [Fact]
        [Trait("FindFwInit", "Success")]
        public void FindFwInitSuccess_Should_ReturnValue()
        {
            //Arrange
            DTO.FwInit  expected = new DTO.FwInit { KeyName = "FORMATDATE", Value = "DD/MM/YYYY" }; 
            var _repo = new Mock<IFwRepo>();
            _repo.Setup(exp => exp.FindFwInitInternal("DEV", "FORMATDATE")).Returns(expected);
            FwService _service = new FwService(null,_repo.Object);

            //Act
            DTO.FwInit actual = _service.FindFwInit(new LabCommonLib.Model.LookupDataRequest() { Connection = "DEV", TextSearch = "FORMATDATE"});

            //Assert 
            _repo.Verify(exp => exp.FindFwInitInternal("DEV", "FORMATDATE"), Times.Once); 
            Assert.Equal(expected.KeyName, actual.KeyName);
            Assert.Equal(expected.Value, actual.Value);
        }

        [Trait("FindFwInit", "Fail")]
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("XXX")]
        public void FindFwInitNotFound_Should_ReturnNull(string KeyName)
        {
            //Arrange
            DTO.FwInit expected = null;
            var _repo = new Mock<IFwRepo>();
            _repo.Setup(exp => exp.FindFwInitInternal("DEV", KeyName)).Returns(expected);
            FwService _service = new FwService(null, _repo.Object);

            //Act
            DTO.FwInit actual = _service.FindFwInit(new LabCommonLib.Model.LookupDataRequest() { Connection = "DEV", TextSearch = KeyName });

            //Assert 
            _repo.Verify(exp => exp.FindFwInitInternal("DEV", KeyName), Times.Once);
            Assert.Equal(expected , actual ); 
        }


        [Trait("FindFwInit", "Fail")]
        [Theory]
        [InlineData(null,"FORMATDATE")]
        [InlineData("", "FORMATDATE")] 
        public void FindFwInitConnectionFail_Should_ThrowExecption(string Connection,string KeyName)
        {
            //Arrange 
            var _repo = new Mock<IFwRepo>(); 
            FwService _service = new FwService(null, _repo.Object);
              
            //Assert  
            Assert.Throws<ArgumentNullException>(() => _service.FindFwInit(new LabCommonLib.Model.LookupDataRequest() { Connection = Connection, TextSearch = KeyName }));

        }
    }
}