export class KRAScaleData
{
    KRAScaleMasterID:number;
    ScaleLevel?:string;
    MinimumScale:number;
    MaximumScale?:number;
}

export class KRAScaleMaster extends KRAScaleData 
{
  KRAScaleTitle: string;
  KRAScaleDetails: KRAScaleDetails[];
}

export class KRAScaleDetails 
{
  KRAScaleDetailId?: number;
  KRAScale?: number;
  ScaleDescription?: string;
}