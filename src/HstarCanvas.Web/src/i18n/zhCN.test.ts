import { describe, expect, it } from 'vitest';
import { zhCN } from './zhCN';

const hasChinese = /[\u4e00-\u9fff]/;

describe('zhCN', () => {
  it('keeps required MVP labels in Simplified Chinese', () => {
    expect(zhCN.commands.runWorkflow).toMatch(hasChinese);
    expect(zhCN.commands.providerSettings).toMatch(hasChinese);
    expect(zhCN.panels.nodeLibrary).toMatch(hasChinese);
    expect(zhCN.panels.inspector).toMatch(hasChinese);
    expect(zhCN.nodes.imageGenerate).toMatch(hasChinese);
    expect(zhCN.status.ready).toMatch(hasChinese);
  });
});

