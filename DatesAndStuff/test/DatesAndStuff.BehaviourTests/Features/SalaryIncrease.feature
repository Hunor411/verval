Feature: Salary increase

    Scenario: Successfully increase salary with acceptable percentage
        Given a person has a salary of 5000
        When the salary is increased by 5 percent
        Then the operation should be successful
        And the new salary should be 5250

    Scenario: Successfully decrease salary within allowed limit
        Given a person has a salary of 5000
        When the salary is increased by -9 percent
        Then the operation should be successful
        And the new salary should be 4550

    Scenario Outline: Adjusting a person's salary by a given percentage
        Given a person has a salary of <InitialSalary>
        When the salary is increased by <Percentage> percent
        Then the operation should <Result>
        And the new salary should be <ExpectedSalary>

        Examples:
            | InitialSalary | Percentage | Result        | ExpectedSalary |
            | 1000          | 10         | be successful | 1100           |
            | 2000          | -5         | be successful | 1900           |
            | 1500          | -15        | fail          | 1500           |
            | 1500          | -10        | fail          | 1500           |
