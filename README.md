# Dust

Welcome!

## Code Quality by codacy.com

- Require completely disable `RemarkLint 7.0.0` tool
- Require disable next code patterns in tool `Sonar C# 8.12`:
  - Control structures should use curly braces (`SonarCSharp_S121`)
  - "switch" statements should have at least 3 "case" clauses (`SonarCSharp_S1301`)
  - Members should not be initialized to default values (`SonarCSharp_S3052`)
  - "out" and "ref" parameters should not be used (`SonarCSharp_S3874`)
  - Empty "case" clauses that fall through to the "default" should be omitted (`SonarCSharp_S3458`)
