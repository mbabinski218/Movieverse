using Movieverse.Domain.AggregateRoots;
using Movieverse.Domain.ValueObjects.Ids.AggregateRootIds;
using NUnit.Framework;

namespace Movieverse.UnitTests.DomainUnitTests;

public class PlatformUnitTests
{
	[Test]
	public void Create_ShouldInstantiatePlatform_WithGivenValues()
	{
		// Arrange
		var id = PlatformId.Create();
		const string name = "TestPlatform";
		const decimal price = 9.99m;

		// Act
		var platform = Platform.Create(id, name, price);

		// Assert
		Assert.IsNotNull(platform);
		Assert.AreEqual(name, platform.Name);
		Assert.AreEqual(price, platform.Price);
	}

	[Test]
	public void Create_WithoutId_ShouldGenerateNewId()
	{
		// Arrange
		const string name = "TestPlatform";
		const decimal price = 9.99m;

		// Act
		var platform = Platform.Create(name, price);

		// Assert
		Assert.IsNotNull(platform.Id);
	}
	
	[Test]
	public void AddMedia_ShouldAddMediaIdToList()
	{
		// Arrange
		var platform = Platform.Create("TestPlatform", 9.99m);
		var mediaId = MediaId.Create();

		// Act
		platform.AddMedia(mediaId);

		// Assert
		Assert.Contains(mediaId, platform.MediaIds.ToList());
	}

	[Test]
	public void RemoveMedia_ShouldRemoveMediaIdFromList()
	{
		// Arrange
		var platform = Platform.Create("TestPlatform", 9.99m);
		var mediaId = MediaId.Create();
		platform.AddMedia(mediaId);

		// Act
		platform.RemoveMedia(mediaId);

		// Assert
		Assert.IsFalse(platform.MediaIds.Contains(mediaId));
	}
	
	[Test]
	public void Equals_ShouldBeTrue_ForSameId()
	{
		// Arrange
		var id = PlatformId.Create();
		var platform1 = Platform.Create(id, "Platform1", 9.99m);
		var platform2 = Platform.Create(id, "Platform2", 19.99m);

		// Act & Assert
		Assert.IsTrue(platform1.Equals(platform2), "Platforms with the same ID should be equal.");
	}

	[Test]
	public void GetHashCode_ShouldBeEqual_ForSameId()
	{
		// Arrange
		var id = PlatformId.Create();
		var platform1 = Platform.Create(id, "Platform1", 9.99m);
		var platform2 = Platform.Create(id, "Platform2", 19.99m);

		// Act & Assert
		Assert.AreEqual(platform1.GetHashCode(), platform2.GetHashCode(), "Hash codes should be equal for platforms with the same ID.");
	}
}