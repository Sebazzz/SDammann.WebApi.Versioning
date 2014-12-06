namespace SDammann.WebApi.Versioning.Tests.Discovery
{
    using System;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using TestSupport.Version1;
    using TestSupport.Version1.Version;
    using Versioning.Discovery;

    [TestClass]
    public class DefaultAttributeControllerVersionDetectorTests {
        [TestMethod]
        public void NamespaceControllerVersionDetector_DetectsCatControllerVersion() {
            // given
            IControllerVersionDetector detector = new DefaultAttributeControllerVersionDetector();
            Type catControllerType = typeof(CatControllerInVersionNamespace);

            // when
            SemVerApiVersion version = detector.GetVersion(catControllerType) as SemVerApiVersion;

            // assert
            Assert.IsNotNull(version, "Expected a SemApiVersion instance to be detected");
            Assert.AreEqual(new Version(6, 5, 3, 2), version.Version);
        }

        [TestMethod]
        public void NamespaceControllerVersionDetector_DetectsNoCatControllerVersion()
        {
            // given
            IControllerVersionDetector detector = new DefaultAttributeControllerVersionDetector();
            Type catControllerType = typeof(CatController);

            // when
            ApiVersion version = detector.GetVersion(catControllerType);

            // assert
            Assert.AreSame(UndefinedApiVersion.Instance, version, "Expected a no SemApiVersion instance to be detected");
        }
    }
}
