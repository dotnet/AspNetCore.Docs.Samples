using IResultImplementation.Data;
using IResultImplementation.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace IResultImplementation.Tests
{
    [TestClass]
    public class IResultImplementationTest
    {
        public IQueryable<Contact>? Data { get; set; }
        public Mock<DbSet<Contact>>? MockSet { get; set; }
        [TestInitialize]
        public void Setup()
        {
            Data = new List<Contact>()
            {
                new Contact() { Id = 1, Name = "John", Email = "john@tmail.com", PhoneNumber = "1234567890" },
                new Contact() { Id = 2, Name = "Jane", Email = "Jane@kam.com", PhoneNumber = "29384736273" },
                new Contact() {Id = 3, Name = "Kahn", Email = "ema@email.com", PhoneNumber = "23239029202"}
            }.AsQueryable();

            MockSet = new Mock<DbSet<Contact>>();
            MockSet.As<IQueryable<Contact>>().Setup(m => m.Provider).Returns(Data.Provider);
            MockSet.As<IQueryable<Contact>>().Setup(m => m.Expression).Returns(Data.Expression);
            MockSet.As<IQueryable<Contact>>().Setup(m => m.ElementType).Returns(Data.ElementType);
            MockSet.As<IQueryable<Contact>>().Setup(m => m.GetEnumerator()).Returns(() => Data.GetEnumerator());
        }
        [TestMethod]
        public async Task GetContactReturnsContactsFromDatabase()
        {
            //Arrange
            var mockContext = new Mock<IResultImplementationContext>();
            mockContext.Setup(c => c.Contact).Returns(MockSet!.Object);
            int expectedStatusCode = 200;
            int expectedItemCount = 3;

            //Act
            var result = (Ok<List<Contact>>)await ContactsHandler.GetContacts(mockContext.Object);

            //Assert
            Assert.AreEqual(expectedStatusCode, result.StatusCode);
            Assert.AreEqual(expectedItemCount, result.Value?.Count);

        }

        [TestMethod]
        public async Task CreateTodoSavesContactToDatabase()
        {
            //Arrange
            var mockDbContextOptions = new Mock<DbContextOptions<IResultImplementationContext>>();
            var mockContext = new Mock<IResultImplementationContext>();
            mockContext.Setup(c => c.Contact).Returns(MockSet!.Object);
            mockContext.Setup(c => c.Contact.Add(It.IsAny<Contact>())).Callback<Contact>(contact => Data = Data.Append(contact));
            var newContact = new Contact()
            {
                Id = 4,
                Name = "John Doe",
                Email = "akd@omail.com",
                PhoneNumber = "1234567890"
            };
            int expectedStatusCode = 201;
            int expectedItemCount = 4;

            //Act
            var result = (CreatedAtRoute<Contact>)ContactsHandler.PostContact(mockContext.Object, newContact);

            //Assert
            Assert.AreEqual(newContact, result.Value);
            Assert.AreEqual(expectedStatusCode, result.StatusCode);
            Assert.AreEqual(expectedItemCount, Data.Count());
        }
    }
}
