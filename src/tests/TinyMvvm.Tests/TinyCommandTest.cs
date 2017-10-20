using System;
using Xunit;

namespace TinyMvvm.Tests
{
    public class TinyCommandTest
    {
        [Fact]
        public void TinyCommand_Execute_Test()
        {
            // Arrange
            var done = false;
            var cmd = new TinyCommand(() => done = true);

            // Act
            cmd.Execute(null);

            // Assert
            Assert.True(done);
        }
    }
}
