using FluentAssertions;
using Xunit;

namespace LazyPropertyHelperTests
{
  public class CreationTest
  {
    private readonly int _beforeExpensiveInstancesCreatedCount = ExpensiveObject.CreatedCount;
    private int ExpensiveInstancesCreated => ExpensiveObject.CreatedCount - _beforeExpensiveInstancesCreatedCount;
    
    private readonly int _beforeServiceInstancesCreatedCount = SampleService.CreatedCount;
    private int ServiceInstancesCreated => SampleService.CreatedCount - _beforeServiceInstancesCreatedCount;

    [Fact]
    public void LazyPropertyIsNotInitializedOnConstruction()
    {
      new SampleService();

      ServiceInstancesCreated.Should().Be(1);
      ExpensiveInstancesCreated.Should().Be(0);
    }
    
    [Fact]
    public void LazyPropertyIsInitializedWhenWorkNeedsToBeDone()
    {
      new SampleService().DoWork(1);

      ServiceInstancesCreated.Should().Be(1);
      ExpensiveInstancesCreated.Should().Be(1);
    }
    
    [Fact]
    public void LazyPropertyIsInitializedWhenRead()
    {
      var load = new SampleService().ExpensiveLoad;

      ServiceInstancesCreated.Should().Be(1);
      ExpensiveInstancesCreated.Should().Be(1);
    }
    
    [Fact]
    public void LazyPropertyIsInitializedWhenReadOnlyOnce()
    {
      var service = new SampleService();

      var load1 = service.ExpensiveLoad;
      var load2 = service.ExpensiveLoad;
      var load3 = service.ExpensiveLoad;

      ServiceInstancesCreated.Should().Be(1);
      ExpensiveInstancesCreated.Should().Be(1);
    }
    
    [Fact]
    public void LazyPropertyIsInitializedOnlyOnce()
    {
      var service = new SampleService();
      
      service.DoWork(1);
      service.DoWork(1);
      service.DoWork(1);

      ServiceInstancesCreated.Should().Be(1);
      ExpensiveInstancesCreated.Should().Be(1);
    }

    [Fact]
    public void LazyPropertyIsInitializedOncePerInstance()
    {
      var service1 = new SampleService();
      var service2 = new SampleService();
      
      service1.DoWork(1);
      service1.DoWork(1);
      service1.DoWork(1);

      service2.DoWork(1);
      service2.DoWork(1);
      service2.DoWork(1);
      
      ServiceInstancesCreated.Should().Be(2);
      ExpensiveInstancesCreated.Should().Be(2);
    }
  }
}