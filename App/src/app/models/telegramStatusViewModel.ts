export interface TelegramStatusViewModel {
    TelegramBotClientStatus: number;
    WebhookStatus: number;
    WebhookUpdateDate: Date;
    WebhookUrl: string;
    WebhookHasCustomCertificate: boolean;
    WebhookPendingUpdateCount: number;
    WebhookLastErrorDate: Date;
    WebhookLastErrorMessage: string;
    WebhookMaxConnections: number;
    WebhookAllowedUpdates: string;
}