import { TelegramServiceStatusPipe } from './telegram-service-status.pipe';

describe('TelegramServiceStatusPipe', () => {
  it('create an instance', () => {
    const pipe = new TelegramServiceStatusPipe();
    expect(pipe).toBeTruthy();
  });
});
