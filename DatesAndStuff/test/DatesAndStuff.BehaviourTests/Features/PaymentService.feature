Feature: Paymet Process
    The payment service must follow a strict pricess for subscription payments.

    Scenario: Payment succeeds when balance is sufficient
        Given the user starts the payment process with sufficient balance of 550
        When the user attempts to pay the subscription fee
        Then the payment should be successful
        And the payment should be confirmed

    Scenario: Payment fails when balance is insufficient
        Given the user starts the payment process with insufficient balance of 450
        When the user attempts to pay the subscription fee
        Then the payment should fail due to insufficient balance
        And the payment should be cancelled
