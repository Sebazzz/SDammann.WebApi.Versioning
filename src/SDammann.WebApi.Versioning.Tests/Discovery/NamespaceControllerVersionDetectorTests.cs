namespace SDammann.WebApi.Versioning.Tests.Discovery {
    using System;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using TestSupport;
    using TestSupport.Version1;
    using TestSupport.Version1.Version;
    using TestSupport.Version1.Version2_5;
    using Versioning.Discovery;

    [TestClass]
    public class NamespaceControllerVersionDetectorTests {
        [TestMethod]
        public void NamespaceControllerVersionDetector_DetectsCatControllerVersion() {
            // given
            IControllerVersionDetector detector = new DefaultControllerVersionDetector();
            Type catControllerType = typeof(CatController);

            // when
            SemVerApiVersion version = detector.GetVersion(catControllerType) as SemVerApiVersion;

            // assert
            Assert.IsNotNull(version, "Expected a SemApiVersion instance to be detected");
            Assert.AreEqual(new Version(1,0), version.Version);
        }

        [TestMethod]
        public void NamespaceControllerVersionDetector_DetectsCatControllerVersion_InDoubleNamespaceWithVersionPrefixes()
        {
            // given
            IControllerVersionDetector detector = new DefaultControllerVersionDetector();
            Type catControllerType = typeof(CatControllerInVersionNamespace);

            // when
            SemVerApiVersion version = detector.GetVersion(catControllerType) as SemVerApiVersion;

            // assert
            Assert.IsNotNull(version, "Expected a SemApiVersion instance to be detected");
            Assert.AreEqual(new Version(1, 0), version.Version);
        }

        [TestMethod]
        public void NamespaceControllerVersionDetector_DetectsCatControllerVersion_InTwoPartVersionNamespace()
        {
            // given
            IControllerVersionDetector detector = new DefaultControllerVersionDetector();
            Type catControllerType = typeof(VersionTwoDotFiveController);

            // when
            SemVerApiVersion version = detector.GetVersion(catControllerType) as SemVerApiVersion;

            // assert
            Assert.IsNotNull(version, "Expected a SemApiVersion instance to be detected");
            Assert.AreEqual(new Version(2, 5), version.Version);
        }

        [TestMethod]
        public void NamespaceControllerVersionDetector_DetectsNoVersion_InNoNamespaceController()
        {
            // given
            IControllerVersionDetector detector = new DefaultControllerVersionDetector();
            Type catControllerType = typeof(ControllerWithNoNamespace);

            // when
            ApiVersion version = detector.GetVersion(catControllerType);

            // assert
            Assert.AreSame(UndefinedApiVersion.Instance, version, "Expected no version to be detected");
        }

        [TestMethod]
        public void NamespaceControllerVersionDetector_DetectsNoVersion_InNoVersionNamespaceController()
        {
            // given
            IControllerVersionDetector detector = new DefaultControllerVersionDetector();
            Type catControllerType = typeof(ControllerWithNoVersionNamespace);

            // when
            ApiVersion version = detector.GetVersion(catControllerType);

            // assert
            Assert.AreSame(UndefinedApiVersion.Instance, version, "Expected no version to be detected");
        }
    }
}