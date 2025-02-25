namespace Sample.Api.Handlers;

public record TestResultRequest(int Number) : IRequest<TestResult>;
public record TestResult;


[ScopedHandler]
[MediatorHttpPost("/testresult", Name = "TestResult")]
public class TestResultRequestHandler : IRequestHandler<TestResultRequest, TestResult>
{
    public Task<TestResult> Handle(TestResultRequest request, CancellationToken cancellationToken)
    {
        return Task.FromResult(new TestResult());
    }
}