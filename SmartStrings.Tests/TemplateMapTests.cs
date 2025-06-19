using Shouldly;

namespace SmartStrings.Tests
{
    public class TemplateMapTests
    {
        private class User
        {
            public Guid Id { get; set; }
            public string? FullName { get; set; }
        }

        private class Request
        {
            public string? Company { get; set; }
            public User? User { get; set; }
        }

        [Fact]
        public void Should_Fill_With_Manually_Bound_Properties()
        {
            var request = new Request
            {
                Company = "SmartCo",
                User = new User
                {
                    Id = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                    FullName = "Jonatas"
                }
            };

            const string template = "{USERNAME}-{COMPANY}-{ID}";

            var result = template.Fill(request, map =>
            {
                map.Bind("USERNAME", r => r.User!.FullName);
                map.Bind("ID", r => r.User!.Id);
            });

            result.ShouldBe("Jonatas-SmartCo-aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa");
        }

        [Fact]
        public void Should_Use_Fallback_When_Bound_Value_Is_Null()
        {
            var request = new Request { User = new User { FullName = null } };
            const string template = "Welcome, {USERNAME:Guest}";

            var result = template.Fill(request, map =>
            {
                map.Bind("USERNAME", r => r.User?.FullName);
            });

            result.ShouldBe("Welcome, Guest");
        }

        [Fact]
        public void Should_Use_Empty_When_Bound_Value_Is_Null_And_No_Fallback()
        {
            var request = new Request { User = new User { FullName = null } };
            const string template = "Welcome, {USERNAME}";

            var result = template.Fill(request, map =>
            {
                map.Bind("USERNAME", r => r.User?.FullName);
            });

            result.ShouldBe("Welcome, ");
        }

        [Fact]
        public void Should_Override_Flat_Object_Values_When_Manually_Bound()
        {
            var request = new Request
            {
                Company = "WrongValue",
                User = new User { FullName = "Correct", Id = Guid.Empty }
            };

            const string template = "Company: {COMPANY}";

            var result = template.Fill(request, map =>
            {
                map.Bind("COMPANY", _ => "SmartStrings Inc.");
            });

            result.ShouldBe("Company: SmartStrings Inc.");
        }

        [Fact]
        public void Should_Handle_Multiple_Bindings_Correctly()
        {
            var newId = Guid.NewGuid();

            var request = new Request
            {
                Company = "DevCorp",
                User = new User { FullName = "Ana", Id = newId }
            };

            const string template = "User {USERNAME} from {COMPANY} has ID {ID}";

            var result = template.Fill(request, map =>
            {
                map.Bind("USERNAME", r => r.User!.FullName);
                map.Bind("COMPANY", r => r.Company);
                map.Bind("ID", r => r.User!.Id);
            });

            result.ShouldContain($"User Ana from DevCorp has ID {newId}");
        }

        [Fact]
        public void Should_Support_Primitive_Model()
        {
            const string template = "Value: {VAL}";

            var result = template.Fill(123, map =>
            {
                map.Bind("VAL", n => n.ToString());
            });

            result.ShouldBe("Value: 123");
        }

        [Fact]
        public void Should_Support_Single_Primitive_Value()
        {
            const string template = "Value: {VAL}";

            var result = TemplateString.Fill(template, 123);

            result.ShouldBe("Value: 123");
        }

        [Fact]
        public void Should_Use_Fallback_For_All_Placeholders()
        {
            var request = new Request { User = null };
            const string template = "User: {USERNAME:unknown}, ID: {ID:0000}, Company: {COMPANY:n/a}";

            var result = template.Fill(request, map =>
            {
                map.Bind("USERNAME", r => r.User?.FullName);
                map.Bind("ID", r => r.User?.Id);
                map.Bind("COMPANY", r => r.Company);
            });

            result.ShouldBe("User: unknown, ID: 0000, Company: n/a");
        }

        [Fact]
        public void Should_Work_With_TemplateString_Static_API()
        {
            var request = new Request
            {
                Company = "SmartBrand",
                User = new User { Id = Guid.Empty, FullName = "Joana" }
            };

            const string template = "Hello {USERNAME} from {COMPANY}";

            var result = TemplateString.Fill(template, request, map =>
            {
                map.Bind("USERNAME", r => r.User!.FullName);
            });

            result.ShouldBe("Hello Joana from SmartBrand");
        }

        [Fact]
        public void Fill_WithSinglePlaceholder_ReplacesCorrectly()
        {
            var template = "Hello {name}";
            var result = TemplateString.Fill(template, "John");
            result.ShouldBe("Hello John");
        }

        [Fact]
        public void Fill_WithMultipleValues_ReplacesInOrder()
        {
            var template = "Hi {0}, welcome to {1}";
            var result = TemplateString.Fill(template, "Alice", "Wonderland");
            result.ShouldBe("Hi Alice, welcome to Wonderland");
        }

        [Fact]
        public void Fill_WithDictionary_ReplacesNamedPlaceholders()
        {
            var template = "Hello {user}, your plan is {plan}";
            var values = new Dictionary<string, string?>
            {
                { "user", "Lucas" },
                { "plan", "Premium" }
            };

            var result = TemplateString.Fill(template, values);
            result.ShouldBe("Hello Lucas, your plan is Premium");
        }

        [Fact]
        public void Fill_WithObject_ReplacesPlaceholdersWithPropertyValues()
        {
            var template = "User: {Name}, Plan: {Plan}";
            var result = TemplateString.Fill(template, new { Name = "Joana", Plan = "Basic" });

            result.ShouldBe("User: Joana, Plan: Basic");
        }

        [Fact]
        public void Fill_WithFallback_UsesFallbackWhenKeyIsMissing()
        {
            var template = "Hi {name:User}, welcome!";
            var result = TemplateString.Fill(template, new Dictionary<string, string?>());

            result.ShouldBe("Hi User, welcome!");
        }

        [Fact]
        public void Fill_WithNullValue_UsesFallback()
        {
            var template = "Hi {name:Guest}";
            var result = TemplateString.Fill(template, new Dictionary<string, string?> { ["name"] = null });

            result.ShouldBe("Hi Guest");
        }

        [Fact]
        public void Fill_WithEmptyTemplate_ReturnsEmptyString()
        {
            var result = TemplateString.Fill("", "test");
            result.ShouldBeEmpty();
        }

        [Fact]
        public void Fill_WithNoPlaceholders_ReturnsOriginalString()
        {
            var result = TemplateString.Fill("Just a normal string", "ignored");
            result.ShouldBe("Just a normal string");
        }

        [Fact]
        public void Fill_WithNullTemplate_ReturnTheNullTemplate()
        {
            string? template = null;

            var act = TemplateString.Fill(template, "value");
            act.ShouldBeNull();
        }

        [Fact]
        public void Fill_WithNullObject_ReplacesWithFallbacksOrEmpty()
        {
            var template = "Hello {name:Guest}, welcome!";
            object? nullValue = null;
            var result = TemplateString.Fill(template, nullValue!);

            result.ShouldBe("Hello Guest, welcome!");
        }

        [Fact]
        public void Fill_WithExtraValues_IgnoresThem()
        {
            var template = "Hello {0}";
            var result = TemplateString.Fill(template, "Alice", "Extra", "More");

            result.ShouldBe("Hello Alice");
        }

        [Fact]
        public void Fill_WithFewerValues_LeavesPlaceholderUnchanged()
        {
            var template = "Hello {0}, welcome to {1}";
            var result = TemplateString.Fill(template, "Alice");

            result.ShouldBe("Hello Alice, welcome to {1}");
        }

        [Fact]
        public void Fill_WithSpecialCharactersInValues_ReplacesCorrectly()
        {
            var template = "Path: {path}";
            var result = TemplateString.Fill(template, new { path = @"C:\Users\Name" });

            result.ShouldBe(@"Path: C:\Users\Name");
        }

        [Fact]
        public void Fill_WithBracesInsideValues_DoesNotBreak()
        {
            var template = "Log: {message}";
            var result = TemplateString.Fill(template, new { message = "Something {weird} happened" });

            result.ShouldBe("Log: Something {weird} happened");
        }
    }
}