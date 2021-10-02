using NUnit.Framework;

namespace UnitTest {

	public class CircularBufferTest {

		[Test]
		public void EmptyBuffer_test() {
			CircularBuffer<string> circularBuffer = new CircularBuffer<string>(2);
			Assert.That(circularBuffer.IsNull());
			circularBuffer.Next();
			Assert.That(circularBuffer.IsNull());
			circularBuffer.Next();
			Assert.That(circularBuffer.IsNull());
		}

		[Test]
		public void Next_test() {
			CircularBuffer<string> circularBuffer = new CircularBuffer<string>(2);
			circularBuffer.Insert("Test");
			Assert.That(circularBuffer.Next(), Is.Null);
			Assert.That(circularBuffer.Next(), Is.EqualTo("Test"));
		}

		[Test]
		public void FullBuffer_test() {
			CircularBuffer<string> circularBuffer = new CircularBuffer<string>(2);
			circularBuffer.Insert("Test1");
			circularBuffer.Insert("Test2");
			Assert.That(circularBuffer.Next(), Is.EqualTo("Test1"));
			Assert.That(circularBuffer.Next(), Is.EqualTo("Test2"));
			Assert.That(circularBuffer.Next(), Is.EqualTo("Test1"));
		}
	}
}
