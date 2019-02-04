using NSubstitute;
using NUnit.Framework;

namespace CleverCrow.UiNodeBuilder.Editors {
    public class NodeTest {
        private Node _node;

        [SetUp]
        public void BeforeEach () {
            _node = new Node();
        } 
        
        public class PurchasedProperty : NodeTest {
            [Test]
            public void Invokes_OnPurchase_if_true_is_set () {
                var result = false;
                _node.OnPurchase.AddListener((node) => result = true);

                _node.Purchased = true;
                
                Assert.IsTrue(result);
            }
            
            [Test]
            public void It_does_not_trigger_OnPurchase_again_if_already_purchased () {
                _node.Purchased = true;
                
                var result = false;
                _node.OnPurchase.AddListener((node) => result = true);
                _node.Purchased = true;

                Assert.IsFalse(result);
            }
            
            [Test]
            public void Calling_purchase_triggers_Enable_on_all_children_who_are_purchasable () {
                var child = Substitute.For<INode>();
                child.IsPurchasable.Returns(true); 
                _node.Children.Add(child);

                _node.Purchased = true;

                child.Received(1).Enable();
            }
            
            [Test]
            public void Calling_purchase_does_not_triggers_children_who_are_not_purchasable () {
                var child = Substitute.For<INode>();
                child.IsPurchasable.Returns(false); 
                _node.Children.Add(child);

                _node.Purchased = true;

                child.Received(0).Enable();
            }
        }

        public class IsPurchasableProperty : NodeTest {
            [Test]
            public void Returns_true_if_Purchased_is_false () {                
                Assert.IsTrue(_node.IsPurchasable);
            }
            
            [Test]
            public void Returns_false_if_Purchased_is_true () {
                _node.Purchased = true;

                Assert.IsFalse(_node.IsPurchasable);
            }

            [Test]
            public void Returns_false_if_Purchased_is_false_and_OnIsPurchasable_is_false () {
                _node.OnIsPurchasable = (node) => false;

                Assert.IsFalse(_node.IsPurchasable);
            }
            
            [Test]
            public void Returns_true_if_Purchased_is_false_and_OnIsPurchasable_is_true () {
                _node.OnIsPurchasable = (node) => true;

                Assert.IsTrue(_node.IsPurchasable);
            }
        }
    }
}