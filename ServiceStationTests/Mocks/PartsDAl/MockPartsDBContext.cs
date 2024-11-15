using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Moq;
using PARTS.DAL.Data;
using PARTS.DAL.Entities;

namespace ServiceStationTests.Mocks.PartsDAl
{
    public class MockPartsDBContext<T> where T : Base
    {
        public Mock<PartsDBContext> _contextMock;
        public Mock<DbSet<T>> _dbSetMock;
        public List<T> mockList = new();
        private readonly object _lock = new();

        private Mock<DbSet<T>> CreateDbSetMock()
        {
            var mock = new Mock<DbSet<T>>();

            mock.As<IQueryable<T>>().Setup(m => m.Provider).Returns(mockList.AsQueryable().Provider);
            mock.As<IQueryable<T>>().Setup(m => m.Expression).Returns(mockList.AsQueryable().Expression);
            mock.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(mockList.AsQueryable().ElementType);
            mock.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(() =>
            {
                lock (_lock)
                {
                    return mockList.AsQueryable().GetEnumerator();
                }
            });

            mock.Setup(d => d.Add(It.IsAny<T>())).Returns((T entity) =>
            {
                lock (_lock)
                {
                    mockList.Add(entity);
                }
                return null;
            });

            mock.Setup(d => d.Update(It.IsAny<T>())).Returns((T entity) =>
            {
                lock (_lock)
                {
                    var entityToRemove = mockList.Find(x => x.Id == entity.Id);
                    mockList.Remove(entityToRemove);
                    mockList.Add(entity);
                }
                return null;
            });

            mock.Setup(d => d.Remove(It.IsAny<T>())).Callback<T>((entity) =>
            {
                lock (_lock)
                {
                    var entityToRemove = mockList.Find(x => x.Id == entity.Id);
                    mockList.Remove(entityToRemove);
                }
            }).Returns((EntityEntry<T>)null);

            mock.Setup(d => d.AddAsync(It.IsAny<T>(), It.IsAny<CancellationToken>())).ReturnsAsync((T entity, CancellationToken token) =>
            {
                lock (_lock)
                {
                    mockList.Add(entity);
                }
                return null;
            });

            mock.Setup(d => d.FindAsync(It.IsAny<object[]>())).ReturnsAsync((object[] ids) =>
            {
                lock (_lock)
                {
                    Guid id = (Guid)ids[0];
                    return mockList.Find(p => p.Id == id);
                }
            });

            return mock;
        }

        public MockPartsDBContext(List<T> testObjects)
        {
            _contextMock = new Mock<PartsDBContext>();
            mockList = testObjects;
        }

        public PartsDBContext GetPartsDBContext()
        {
            _dbSetMock = CreateDbSetMock();
            _contextMock.Setup(x => x.Set<T>()).Returns(_dbSetMock.Object);
            return _contextMock.Object;
        }
    }
}
