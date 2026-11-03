# Alerting

Terms describing user-configured notifications about world events.

## Language

**User**:
A human individual who owns their notification preferences.
_Avoid_: Organization, shared account (when referring to a user)

**Alert rule**:
A user's selection of event types they want notifications about and the channels through which they want those notifications. Each user has at most one alert rule and can update it.
_Avoid_: Alert (when referring to the saved rule rather than a notification)

**Event type**:
A category from a predefined list used to describe a world event and select it in an alert rule.
_Avoid_: Keyword, topic (when referring to an event type)

**Event**:
A reported occurrence in the world about which users may want to be notified.
_Avoid_: Notification (when referring to the occurrence itself)

**Notification**:
A message about an event intended for one user. The same notification can reach that user through more than one channel.
_Avoid_: Alert rule (when referring to the message)

**Delivery attempt**:
One try to send a notification through one channel.
_Avoid_: Notification (when referring to an individual sending attempt)
