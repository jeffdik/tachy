require 'pp'

require 'spec'

require 'dndn'

describe "dndn" do
  include Dndn
  
  it "should translate a constructor" do
    dndn([:"Form."]).should == [:new, [:quote, :Form]]
  end

  it "should translate a constructor subform" do
    dndn([:define, :"first-form", [:"Form."]]).should == [:define, :"first-form", [:new, [:quote, :Form]]]
  end

  it "should translate setting of properties" do
    dndn([:".Text$", :"first-form", "My First Form"]).should == [:"set-property", :"first-form", [:quote, :Text], "My First Form"]
    dndn([:".Width$", :"first-form", 200]).should == [:"set-property", :"first-form", [:quote, :Width], 200]
    dndn([:".Height$", :"first-form", 100]).should == [:"set-property", :"first-form", [:quote, :Height], 100]
  end

  it "should translate class methods" do
    dndn([:"System.Windows.Forms.Application.Run", :"first-form"]).should == [:"call-static", [:quote, :"System.Windows.Forms.Application"], [:quote, :Run], :"first-form"]
  end
    
  it "should translate many sexps" do
    dndn([[:using, "System.Windows.Forms"],
          [:define, :"first-form", [:"Form."]],
          [:".Text$", :"first-form", "My First Form"],
          [:".Width$", :"first-form", 200],
          [:".Height$", :"first-form", 100],
          [:"System.Windows.Forms.Application.Run", :"first-form"]]).should ==
             [[:using, "System.Windows.Forms"],
             [:define, :"first-form", [:new, [:quote, :Form]]],
             [:"set-property", :"first-form", [:quote, :Text], "My First Form"],
             [:"set-property", :"first-form", [:quote, :Width], 200],
             [:"set-property", :"first-form", [:quote, :Height], 100],
             [:"call-static", [:quote, :"System.Windows.Forms.Application"], [:quote, :Run], :"first-form"]]
  end
end
